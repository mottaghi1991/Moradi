using Core.Enums;
using Core.Extention;
using Core.Service.Interface.Dr;
using Core.Service.Interface.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NuGet.Configuration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalSite.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminHomeController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly IUserDiet _userDiet;
        private readonly IPayment _payment;
        public AdminHomeController(IDistributedCache cache, IUserDiet userDiet, IPayment payment)
        {
            _cache = cache;
            _userDiet = userDiet;
            _payment = payment;
        }
        [Route("Admin")]
        public async Task<IActionResult> Index()
        {
            //var Alldiets =  _userDiet.GetUserDietById(99).Result;
            //await _payment.VerifyPayment(authority: Alldiets.PaymentAuthority, (int)Alldiets.Amount, StoreType.Diet, false);
            //var target = Alldiets.OrderByDescending(a=>a.CreateAt).Where(a => a.PaymentDate == null);
            //foreach (var item  in  Alldiets)
            //{
            //    var payevent = await _payment.VerifyPayment(authority: item.PaymentAuthority, (int)item.Amount, StoreType.Diet, false);
            //}



            return RedirectToAction("Index","DietOrder");
        }


    }
}
