using Core.Dto.Shop.Order;
using Core.Extention;
using Core.Interface.Sms;
using Core.Service.Interface.Deliverd;
using Core.Service.Interface.Shop;
using Domain;
using Domain.Shop;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    public class ShopController : BaseController
    {
        private readonly IOrder _order;
        private readonly IDelivery _Delivery;
        private readonly ISms _sms;

        public ShopController(IOrder order, IDelivery delivery, ISms sms)
        {
            _order = order;
            _Delivery = delivery;
            _sms = sms;
        }

        public async Task<IActionResult> Index(int? userId, string fullName, string mobile, string paymentStatus , int pageNumber = 1, int pageSize = 5)
        {
            string paymentStatusFilter = paymentStatus;
            if (string.Equals(paymentStatus, "all", StringComparison.OrdinalIgnoreCase))
            {
                paymentStatusFilter = null;
            }

            var result = await _order.GetPagedOrdersAsync(
            userId,           // همون ورودی کاربر
            paymentStatus,    // بعد از تبدیل "all" به null
            fullName,         // همون ورودی
            mobile,           // همون ورودی
            pageNumber,
            pageSize
        );
            result.paymentStatus = paymentStatus;
            return View(result);




            //var result = await _order.GetPagedOrdersAsync(pageNumber: pageNumber, pageSize: 20, userName: fullName, paymentType: Domain.OrderStatus.Paid);
            //return View(result);


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
                await _sms.ProductSend(order.User.UserName, 1466234, ".");
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            TempData[Error] = ErrorMessage;
            return View(postVm);
        }
        public async Task<IActionResult> Deliverd(int OrderId)
        {
            var order = await _order.GetOrderById(OrderId);
            if(order==null)
            {
                return NotFound();
            }
            if(order.Status!=Domain.OrderStatus.Shipped)
            {
                TempData[Error] = "بسته ارسال نگردیده است";
                return RedirectToAction("SendPostOrder", new { Orderid = OrderId });
            }
            order.Status = Domain.OrderStatus.Delivered;
            var result = await _order.Update(order);
            if (result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            TempData[Error] = ErrorMessage;
            return RedirectToAction("SendPostOrder", new { Orderid = OrderId });
        }
    }
}
