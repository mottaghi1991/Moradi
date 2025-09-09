using Core.Interface.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrMoradi.Controllers
{
    [AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly IProduct _product;

        public ShopController(IProduct product)
        {
            _product = product;
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
    }
}
