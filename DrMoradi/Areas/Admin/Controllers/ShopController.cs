using Core.Service.Interface.Shop;
using Microsoft.AspNetCore.Mvc;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    public class ShopController : BaseController
    {
        private readonly IOrder _order;

        public ShopController(IOrder order)
        {
            _order = order;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _order.GetAllOrder());
        }
    }
}
