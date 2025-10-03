using Core.Interface.Store;
using Core.Service.Interface.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace DrMoradi.Controllers
{
    [AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly IProduct _product;
        private readonly IProvince _province;
        private readonly ICategory _category;

        public ShopController(IProduct product, IProvince province, ICategory category)
        {
            _product = product;
            _province = province;
            _category = category;
        }

        public async Task<IActionResult> Index(int? categoryId, string sort)
        {
            
            ViewBag.Categories = new SelectList(await _category.GetAllByActive(true),"Id", "CategoryName",categoryId);
            var obj = await _product.getByFilter(categoryId, sort);
            return View(obj);
        }
        public async Task<IActionResult> ProductDetail(int ProductId)
        {
            return View(await _product.GetProductById(ProductId));
        }
        public IActionResult Add()
        {
         return View(); 
        }
        [HttpGet]
        public async Task<JsonResult> GetCityByProvinceId(int ProId)
        {
            //  var result = _repoSubSystems.getSubSystemsBySystemCode(SystemId).Select(p => new { ID = p.SystemID, Name = p.Title }).ToList();
            var result = new SelectList(await _province.GetAllCityByProId(ProId),"Id","Title");
            return Json(result);
        }
    }
}
