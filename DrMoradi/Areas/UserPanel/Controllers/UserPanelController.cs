using Core.Dto.ViewModel.User;
using Core.Extention;
using Core.Service.Interface.Dr;
using Core.Service.Interface.Payment;
using Core.Service.Interface.Users;
using Core.Service.Services.Payment;
using Domain;
using Domain.Payment;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStore.Base;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebStore.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    //[Authorize]
    [AllowAnonymous]
    public class UserPanelController : BaseController
    {
        private readonly IPayment _payment;
        private readonly IUserDiet _userDiet;
        private readonly IUser _user;
        private readonly ILogger<UserPanelController> _logger;

        public UserPanelController(IPayment payment, IUserDiet userDiet, IUser user, ILogger<UserPanelController> logger)
        {
            _payment = payment;
            _userDiet = userDiet;
            _user = user;
            _logger = logger;
        }

        [Route("/UserPanel")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("ورود کاربر {UserId} به UserPanel.Index", User.GetUserId());
            return RedirectToAction("MyDiet", "UserDiet");
        }
        public async Task<IActionResult> StartPayment(int UserDietId)
        {
            _logger.LogInformation("شروع فرایند پرداخت برای UserDietId={UserDietId}, UserId={UserId}", UserDietId, User.GetUserId());
            var userdiet = await _userDiet.GetUserDietById(UserDietId);
            if (userdiet == null)
            {
                _logger.LogError("UserDiet با شناسه {UserDietId} یافت نشد. UserId={UserId}", UserDietId, User.GetUserId());
                TempData[Error] = "اطلاعات رژیم پیدا نشد";
                return RedirectToAction("Index");
            }
            string callbackUrl = $"{Request.Scheme}://{Request.Host}/UserPanel/UserPanel/verify";
            var First = await _payment.FirstRequestPayment(UserDietId, (int)userdiet.Amount, callbackUrl, userdiet.diet.Name, "", User.Identity?.Name);
            if (First.data != null)
            {
                _logger.LogInformation("درخواست اولیه پرداخت موفق. Authority={Authority}, Amount={Amount}, UserId={UserId}", First.data.authority, userdiet.Amount, User.GetUserId());
                //return RedirectToAction("SendTOBank", new { Url = "https://zarinpal.com/pg/StartPay/" + First.data.authority });
                return RedirectToAction("SendTOBank", new { Url = "https://sandbox.zarinpal.com/pg/StartPay/" + First.data.authority });
            }
            else
            {
                _logger.LogError("درخواست اولیه پرداخت ناموفق. UserDietId={UserDietId}, UserId={UserId}", UserDietId, User.GetUserId());
                TempData[Error] = "صفحه پرداخت با مشکل مواجه گردیده است";
                return RedirectToAction("Index");
            }
        }
        public IActionResult SendTOBank(string Url)
        {
            _logger.LogInformation("انتقال کاربر {UserId} به درگاه بانکی. URL={Url}", User.GetUserId(), Url);
            LogCookies(HttpContext, "Before Payment Redirect");
            return Redirect(Url);
        }
        [HttpGet]
        public async Task<IActionResult> verify(string Authority, string Status)
        {
            LogCookies(HttpContext, "After Payment Callback");
            _logger.LogInformation("بازگشت از درگاه بانکی. Authority={Authority}, Status={Status}, UserId={UserId}", Authority, Status, User.GetUserId());
            if (Status == "OK")
            {
                var userdiet = await _userDiet.GetUserDietByAuthority(Authority);
                if (userdiet == null)
                {
                    _logger.LogError("هیچ UserDiet مرتبط با Authority={Authority} پیدا نشد. UserId={UserId}", Authority, User.GetUserId());
                    TempData[Error] = "پرداخت پیدا نشد";
                    return RedirectToAction("Index");
                }
                var payevent = await _payment.VerifyPayment(authority: Authority, (int)userdiet.Amount);

                if (payevent)
                {
                    _logger.LogInformation("پرداخت موفق. UserId={UserId}, Amount={Amount}, userdiet={userdiet}, Authority={Authority}",
                 User.GetUserId(), userdiet.Amount, userdiet, Authority);
                    TempData[Success] = " از پرداخت شما متشکریم";
                    // نمایش خطا
                    return RedirectToAction("Index", "UserPanel", "UserPanel");
                }
                else
                {


                    _logger.LogWarning("پرداخت تایید نشد. UserId={UserId}, Amount={Amount}, Authority={Authority}",
                    User.GetUserId(), userdiet.Amount, Authority);
                    TempData[Error] = " پرداخت با مشکل مواجه گردیده است";
                    // نمایش خطا
                    return RedirectToAction("Index", "UserPanel", "UserPanel");
                }
            }
            if (Status == "NOK")
            {
                _logger.LogWarning("پرداخت لغو شد یا ناموفق بود. UserId={UserId}, Authority={Authority}",
          User.GetUserId(), Authority);
                TempData[Error] = " پرداخت با مشکل مواجه گردیده است";
                // نمایش خطا
                return RedirectToAction("Index", "UserPanel", "UserPanel");
            }

            _logger.LogWarning("وضعیت پرداخت موفق. Status={Status}, UserId={UserId}, Authority={Authority}",
           Status, User.GetUserId(), Authority);
            TempData[Success] = " از پرداخت شما متشکریم";
            // نمایش خطا
            return RedirectToAction("Index", "UserPanel", "UserPanel");
        }
        [HttpGet]
        public IActionResult FillForm()
        {
            _logger.LogInformation("ورود کاربر {UserId} به فرم تکمیل اطلاعات کاربری", User.GetUserId());
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FillForm(FillFromVm formVm)
        {
            _logger.LogInformation("ثبت فرم اطلاعات کاربر. UserId={UserId}, FullName={FullName}", User.GetUserId(), formVm.FullName);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("مدل فرم معتبر نیست. UserId={UserId}", User.GetUserId());
                return View(formVm);
            }
            var obj = await _user.GetUserByUserId(User.GetUserId());
            obj.FullName = formVm.FullName;
            obj.City = formVm.City;
            obj.gender = formVm.gender;
            obj.Job = formVm.Job;
            var result = await _user.UpdateAsync(obj);

            var claims = User.Claims.ToList();
            claims.RemoveAll(c => c.Type == ClaimTypes.Name);
            claims.Add(new Claim(ClaimTypes.Name, result.FullName));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            _logger.LogInformation("اطلاعات کاربر {UserId} با موفقیت بروزرسانی شد. FullName={FullName}", User.GetUserId(), result.FullName);
            return RedirectToAction("Index");
        }
        private void LogCookies(HttpContext context, string logPoint)
        {
            var sessionCookie = context.Request.Cookies[".AspNetCore.Session"];
            var authCookie = context.Request.Cookies[".AspNetCore.Cookies"]; // یا اسم Custom
            _logger.LogInformation(EventIdList.APi,"---- COOKIE LOG [{logPoint}] ----", logPoint);
            _logger.LogInformation("Session cookie: {cookieValue}", sessionCookie ?? "NULL");
            _logger.LogInformation("Auth cookie: {cookieValue}", authCookie ?? "NULL");
            _logger.LogInformation("-------------------------------");
        }
    }
      

    }
