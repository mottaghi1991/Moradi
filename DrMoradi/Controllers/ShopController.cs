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

        public ShopController(IProduct product, IProvince province)
        {
            _product = product;
            _province = province;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _product.GetAll());
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
