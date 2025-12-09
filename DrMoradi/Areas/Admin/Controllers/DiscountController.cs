using Core.Service.Interface.Shop;
using Core.Service.Interface.Users;
using Domain.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    [Authorize]
    public class DiscountController : BaseController
    {
        private readonly IDiscount _discount;
        private readonly IUser _user;
        public DiscountController(IDiscount discount, IUser user)
        {
            _discount = discount;
            _user = user;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _discount.GetDiscountsByStatus(null));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.UserId = new SelectList(await _user.GetAllUser(), "UserId", "DisplayName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Discount discount)
        {
            if (discount.Percent == 0|| discount.UserId==0)
            {
                ViewBag.UserId = new SelectList(await _user.GetAllUser(), "UserId", "DisplayName", discount.UserId);
                ModelState.AddModelError("UserId", "وارد کردن هر دو مورد اجباری می باشد .");
                return View(discount);

            }
            var result = await _discount.Insert(discount);
            if (result != null)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(await _user.GetAllUser(), "UserId", "DisplayName", discount.UserId);
            TempData[Error]=ErrorMessage;
            return View(discount);


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int DiscountId)
        {
            var discount = await _discount.GetDiscountById(DiscountId);
            if(discount==null)
            {
                return NotFound();
            }
            ViewBag.UserId = new SelectList(await _user.GetAllUser(), "UserId", "DisplayName", discount.UserId);
            return View(discount);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Discount discount)
        {
            var result=await _discount.update(discount);
            if(result.ErrorId!=0)
            {
                ViewBag.UserId = new SelectList(await _user.GetAllUser(), "UserId", "DisplayName", discount.UserId);
                ModelState.AddModelError("", result.ErrorTitle);
                return View(discount);
            }
            TempData[Success] = SuccessMessage;

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int DiscountId)
        {
            var discount = await _discount.GetDiscountById(DiscountId);
            if (discount == null)
            {
                return NotFound();
            }
            var result = await _discount.Delete(discount);
            if (result.ErrorId != 0)
            {
                TempData[Error] = result.ErrorTitle;
                return RedirectToAction("Index");
            }
            TempData[Success] = SuccessMessage;

            return RedirectToAction("Index");

        }
    }
}
