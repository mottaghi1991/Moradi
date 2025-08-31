using Core.Attribute;
using Core.Dto.ViewModel.User;
using Core.Dto.ViewModel.User.Login;
using Core.Enums;
using Core.Extention;
using Core.Interface.Sms;
using Core.Service.Interface.Users;
using Domain;
using Domain.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.CodeDom.Compiler;

using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebStore.Base;
using static WebStore.Base.BaseController;


namespace DrMoradi.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private IUser _user;
        private IViewRender _viewRender;
        private readonly ILogger _logger;
        private readonly ISms _sms;
        private readonly IUserOtp _userOtp;
        private readonly IMemoryCache _memoryCache;
        public AccountController(IUser user, IViewRender viewRender, ILoggerFactory factory, ISms sms, IUserOtp userOtp, IMemoryCache memoryCache)
        {
            _user = user;
            _viewRender = viewRender;
            _logger = factory.CreateLogger("Session");
            _sms = sms;
            _userOtp = userOtp;
            _memoryCache = memoryCache;
        }


        [HttpGet]
        [Route("SMSLogin")]
        [RediretAuthenticate("/UserPanel")]
        public IActionResult SMSLogin()
        {
            return View();
        }
        [HttpPost]
        [Route("Smslogin")]
        public async Task<IActionResult> Smslogin([FromBody] AcceptCodeViewModel acceptCode)
        {

            var phoneNumber = HttpContext.Session.GetString("PhoneNumber");

            if (!await _userOtp.CanTryAsync(phoneNumber))
                return Json(JsonFailure("تعداد تلاش‌های شما بیش از حد مجاز است.کاربری شما 10 دقیقه غیرفعال گردید."));

            if (!await _userOtp.UseCodeAsync(phoneNumber, acceptCode.SendCode))
            {
                await _userOtp.IncreaseTryAsync(phoneNumber);
                return BadRequest(JsonFailure("کد وارد شده صحیح نیست"));
            }

            var user = await _user.GetOrCreateUser(phoneNumber);
            await _user.SignIn(HttpContext, user);
            await _userOtp.UseCodeAsync(phoneNumber, acceptCode.SendCode);
            _logger.LogWarning(EventIdList.Login, "User with {PhoneNumber} login", phoneNumber);
            var obj=await _user.GetUserByUserNameAsync(user.UserName);
            var redirectUrl = !string.IsNullOrEmpty(acceptCode.ReturnUrl) && Url.IsLocalUrl(acceptCode.ReturnUrl)
     ? acceptCode.ReturnUrl
     : Url.Content("~/");
            string Fullname = (obj?.FullName);
            if(Fullname==null)
                return Json(JsonSuccess("ورود موفق", redirectUrl: "UserPanel/UserPanel/Fillform"));

            else
                return Json(JsonSuccess("ورود موفق", redirectUrl: redirectUrl));

        }

        [HttpPost]
        [Route("SendCode")]
        public async Task<IActionResult> SendCode([FromBody] SmsLoginViModel loginViewModel)
        {
            loginViewModel.PhoneNumber = loginViewModel.PhoneNumber;
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            try
            {
                HttpContext.Session.SetString("PhoneNumber", loginViewModel.PhoneNumber);
                //محدودیت تعداد تلاش‌ها (Rate Limiting):
                string SendKey = $"LastCodeSend_{loginViewModel.PhoneNumber}";
                if (_memoryCache.TryGetValue<DateTime>(SendKey, out var LastSentTime))
                {
                    var SecondPassed = (DateTime.UtcNow - LastSentTime).TotalSeconds;
                    if (SecondPassed < 60)
                    {
                        return BadRequest(new { success = false, message = $"لطفاً {60 - (int)SecondPassed} ثانیه دیگر تلاش کنید." });

                    }

                }
                _memoryCache.Set(SendKey, DateTime.UtcNow, TimeSpan.FromMinutes(2));

                var Code =Core.Extention.CodeGenerator.Generate();
                var response = await _sms.SendSms(loginViewModel.PhoneNumber, 553170, Code);
                if (response == null)
                {
                    return BadRequest(new { success = false, message = "ورود ناموفق. لطفاً دوباره تلاش کنید." });

                }
                var result =await _userOtp.insertAsync(new Domain.SMS.UserOtp()
                {
                    Code = Code,
                    CreateAt = DateTime.UtcNow,
                    IsUsed = false,
                    ExpireAt = DateTime.UtcNow.AddMinutes(2),
                    PhoneNumber = loginViewModel.PhoneNumber
                });
                if (!result)
                {
                    return BadRequest(new { success = false, message = "ارسال پیامک با مشکل مواجه شده است." });
                }
                return Ok(new { success = true, message = "کد با موفقیت ارسال گردید" });
            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, message = "ارسال پیامک با مشکل مواجه شده است." });

            }


        }
        [HttpPost]
        [Route("ReSendCode")]
        public async Task<IActionResult> ReSendCode()
        {
            var phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            var Code = Core.Extention.CodeGenerator.Generate();
            var response = _sms.SendSms(phoneNumber, 553170, Code);
            if (response == null)
            {
                return BadRequest(new { success = false, message = "ورود ناموفق. لطفاً دوباره تلاش کنید." });

            }
            var result =await _userOtp.insertAsync(new Domain.SMS.UserOtp()
            {
                Code = Code,
                CreateAt = DateTime.UtcNow,
                IsUsed = false,
                ExpireAt = DateTime.UtcNow.AddMinutes(2),
                PhoneNumber = phoneNumber
            });
            if (!result)
            {
                return BadRequest(new { success = false, message = "ارسال پیامک با مشکل مواجه شده است." });
            }
            return Ok(new { success = true, message = "کد با موفقیت ارسال گردید" });

        }
      

        
        [Route("Logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        [Route("Login")]
        [RediretAuthenticate("/Admin")]
        public IActionResult Login()
        {
            return View();

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await _user.LoginCheckAsync(loginViewModel);
            if (user == null)
            {
                ModelState.AddModelError("UserName", "نام کاربری یا رمز عبور اشتباه می باشد");
                return View(loginViewModel);
            }
            else
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.ItUserId.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var Properties = new AuthenticationProperties()
                {
                    IsPersistent = loginViewModel.IsRemember
                };
                HttpContext.SignInAsync(principal, Properties);

                return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
            }

        }



        private object JsonSuccess(string message, string redirectUrl) => new
        {
            success = true,
            message,
            redirectUrl
        };

        private object JsonFailure(string message) => new
        {
            success = false,
            message
        };
    }
}

