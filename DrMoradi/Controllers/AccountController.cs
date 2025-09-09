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
            _logger.LogInformation(EventIdList.Login, "SMS login attempt for phone: {PhoneNumber}", phoneNumber);
            if (!await _userOtp.CanTryAsync(phoneNumber))
            {
                _logger.LogWarning(EventIdList.Login, "Too many attempts for {PhoneNumber}. Account locked for 10 min", phoneNumber);
                return Json(JsonFailure("تعداد تلاش‌های شما بیش از حد مجاز است.کاربری شما 10 دقیقه غیرفعال گردید."));

            }

            if (!await _userOtp.UseCodeAsync(phoneNumber, acceptCode.SendCode))
            {
                await _userOtp.IncreaseTryAsync(phoneNumber);
                _logger.LogWarning(EventIdList.Login, "Invalid OTP code entered for {PhoneNumber}", phoneNumber);
                return BadRequest(JsonFailure("کد وارد شده صحیح نیست"));
            }

            var user = await _user.GetOrCreateUser(phoneNumber);
            await _user.SignIn(HttpContext, user);
            await _userOtp.UseCodeAsync(phoneNumber, acceptCode.SendCode);
            _logger.LogInformation(EventIdList.Login, "User {UserId} with phone {PhoneNumber} signed in via SMS", user.ItUserId, phoneNumber); var obj=await _user.GetUserByUserNameAsync(user.UserName);
            var redirectUrl = !string.IsNullOrEmpty(acceptCode.ReturnUrl) && Url.IsLocalUrl(acceptCode.ReturnUrl)
     ? acceptCode.ReturnUrl
     : Url.Content("~/");
            string Fullname = (obj?.FullName);
            if(Fullname==null)
            {
                _logger.LogInformation(EventIdList.Login, "Redirecting user {UserId} to fill profile form", user.ItUserId);
                return Json(JsonSuccess("ورود موفق", redirectUrl: "UserPanel/UserPanel/Fillform"));

            }

            else
            {
                _logger.LogInformation(EventIdList.Login, "User {UserId} redirected to {RedirectUrl}", user.ItUserId, redirectUrl);
                return Json(JsonSuccess("ورود موفق", redirectUrl: redirectUrl));


            }

        }

        [HttpPost]
        [Route("SendCode")]
        public async Task<IActionResult> SendCode([FromBody] SmsLoginViModel loginViewModel)
        {
            _logger.LogInformation(EventIdList.Login, "User requested SendCode for phone: {PhoneNumber}", loginViewModel.PhoneNumber);
            loginViewModel.PhoneNumber = loginViewModel.PhoneNumber;
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(EventIdList.Login, "Invalid model state in SendCode. Phone: {PhoneNumber}", loginViewModel.PhoneNumber);
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
                        _logger.LogWarning(EventIdList.Login, "SendCode too soon for {PhoneNumber}. Wait {Seconds} more seconds", loginViewModel.PhoneNumber, 60 - (int)SecondPassed);
                        return BadRequest(new { success = false, message = $"لطفاً {60 - (int)SecondPassed} ثانیه دیگر تلاش کنید." });

                    }

                }
                _memoryCache.Set(SendKey, DateTime.UtcNow, TimeSpan.FromMinutes(2));

                var Code =Core.Extention.CodeGenerator.Generate();
                var response = await _sms.SendSms(loginViewModel.PhoneNumber, 553170, Code);
                if (response == null)
                {
                    _logger.LogError(EventIdList.Login, "SMS sending failed for {PhoneNumber}", loginViewModel.PhoneNumber);
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
                    _logger.LogError(EventIdList.Login, "Failed to insert OTP for {PhoneNumber}", loginViewModel.PhoneNumber);
                    return BadRequest(new { success = false, message = "ارسال پیامک با مشکل مواجه شده است." });
                }
                _logger.LogInformation(EventIdList.Login, "OTP code sent successfully to {PhoneNumber}", loginViewModel.PhoneNumber);
                return Ok(new { success = true, message = "کد با موفقیت ارسال گردید" });
            }
            catch (Exception e)
            {
                _logger.LogError(EventIdList.Login, e, "Exception in SendCode for {PhoneNumber}", loginViewModel.PhoneNumber);
                return BadRequest(new { success = false, message = "ارسال پیامک با مشکل مواجه شده است." });

            }


        }
        [HttpPost]
        [Route("ReSendCode")]
        public async Task<IActionResult> ReSendCode()
        {
            var phoneNumber = HttpContext.Session.GetString("PhoneNumber");
            _logger.LogInformation(EventIdList.Login, "Resend OTP requested for {PhoneNumber}", phoneNumber);
            var Code = Core.Extention.CodeGenerator.Generate();
            var response = _sms.SendSms(phoneNumber, 553170, Code);
            if (response == null)
            {
                _logger.LogError(EventIdList.Login, "Resend OTP failed for {PhoneNumber}", phoneNumber);
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
                _logger.LogError(EventIdList.Login, "Failed to insert OTP in ReSendCode for {PhoneNumber}", phoneNumber);
                return BadRequest(new { success = false, message = "ارسال پیامک با مشکل مواجه شده است." });
            }
            _logger.LogInformation(EventIdList.Login, "OTP resent successfully to {PhoneNumber}", phoneNumber);
            return Ok(new { success = true, message = "کد با موفقیت ارسال گردید" });

        }
      

        
        [Route("Logout")]
        public IActionResult LogOut()
        {
            _logger.LogInformation(EventIdList.Login, "User logged out. Session cleared");
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        [Route("Login")]
        [RediretAuthenticate("/Admin")]
        public IActionResult Login()
        {
            _logger.LogInformation(EventIdList.Login, "Admin login page requested"); return View();

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(EventIdList.Login, "Invalid model state in admin login"); return View(loginViewModel);
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
                _logger.LogInformation(EventIdList.Login, "Admin user {UserId} logged in successfully", user.ItUserId);
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

