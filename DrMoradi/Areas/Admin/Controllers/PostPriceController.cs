using Core.Dto.ViewModel.main;
using Core.Extention;
using System.Linq;
using Core.Service.Interface.Dr;
using Core.Service.Interface.Shop;
using Domain.Main;
using Microsoft.AspNetCore.Mvc;
using WebStore.Base;
using Domain.Shop;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    public class PostPriceController : BaseController
    {
        private readonly IProvince _province;

        public PostPriceController(IProvince province)
        {
            _province = province;
        }

        public async  Task<IActionResult> Index(int provicesId=0)
        {
            ViewBag.Providce = new SelectList(await _province.GetAll(), "Id", "Title");

            if (provicesId==0)
            {
                return View(await _province.GetAllPrice());

            }
            return View(await _province.GetAllPriceByProvicedid(provicesId));

        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Providce = new SelectList(await _province.GetAll(), "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PostPrice upVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providce = new SelectList(await _province.GetAll(), "Id", "Title",upVm.ProvicesId);

                return View(upVm);
            }
      
            var Result = await _province.InsertPrice(upVm);
            if (Result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            ViewBag.Providce = new SelectList(await _province.GetAll(), "Id", "Title", upVm.ProvicesId);

            TempData[Error] = ErrorMessage;
            return View(upVm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int PriceId)
        {
            if (PriceId <= 0)
                return NotFound();
            var pop = await _province.GetPriceById(PriceId);
            if (pop == null)
                return NotFound();
            ViewBag.Providce = new SelectList(await _province.GetAll(), "Id", "Title", pop.ProvicesId);

            return View(pop);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostPrice postPrice)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Providce = new SelectList(await _province.GetAll(), "Id", "Title", postPrice.ProvicesId);

                return View(postPrice);
            }
             
            
       
            var Result = await _province.UpdatePrice(postPrice);
            if (Result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            ViewBag.Providce = new SelectList(await _province.GetAll(), "Id", "Title", postPrice.ProvicesId);

            TempData[Error] = ErrorMessage;
            return View(postPrice);
        }
        public async Task<IActionResult> Delete(int PriceId)
        {
            var pop = await _province.GetPriceById(PriceId);
            if (pop == null)
                return NotFound();
            var result = await _province.DeletePrice(pop);
            if (result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }

            TempData[Error] = ErrorMessage;
            return RedirectToAction("Index");
        }
    }
}
