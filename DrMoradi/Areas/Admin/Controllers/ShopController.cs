using Core.Dto.Shop.Order;
using Core.Extention;
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
        public async Task<IActionResult> SendPostOrder(int Orderid)
        {
            var obj =await _order.GetOrderById(Orderid);
            if(obj==null)
            {
                return NotFound();
            }
            return View(new OrderSendPostVm()
            {
                Id = Orderid,
                PostIdentity = obj.PostIdentity,
                SendDate = obj.SendDate?.ToPersian() ?? ""
            });
        }
        [HttpPost]
        public async Task<IActionResult> SendPostOrder(OrderSendPostVm postVm)
        {
            
            if (!ModelState.IsValid)
            {
                return View(postVm);
            }
            var order = await _order.GetOrderById(postVm.Id);
            order.SendDate = postVm.SendDate.ToMiladi();
            order.PostIdentity = postVm.PostIdentity;
            order.Status = Domain.OrderStatus.Shipped;
            var result = await _order.Update(order);
            if(result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            TempData[Error] = ErrorMessage;
            return View(postVm);
        }
    }
}
