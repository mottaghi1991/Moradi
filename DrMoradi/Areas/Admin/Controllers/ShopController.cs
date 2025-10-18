using Core.Service.Interface.Deliverd;
using Core.Service.Interface.Shop;
using Microsoft.AspNetCore.Mvc;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    public class ShopController : BaseController
    {
        private readonly IOrder _order;
        private readonly IDelivery _Delivery;

        public ShopController(IOrder order, IDelivery delivery)
        {
            _order = order;
            _Delivery = delivery;
        }

        public async Task<IActionResult> Index()
        {
            var obj = await _order.GetAllOrder();
            return View(obj);
        }

        public async Task<IActionResult> createAlopeyk(int OrderId)
        {
            var obj = await _order.GetOrderById(OrderId);
            if (obj==null)
            {
                return NotFound();
            }

            var result = await _Delivery.CreateAloPeykOrderAsync(obj, obj.ShippingAddress, obj.User);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> OrderDetail(int OrderId)
        {
            return View(await _order.GetOrderById(OrderId));
        }
    }
}
