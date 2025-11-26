using Azure;
using Core.Dto.Shop.Address;
using Core.Enums;
using Core.Extention;
using Core.Interface.Sms;
using Core.Interface.Store;
using Core.Service.Interface.Deliverd;
using Core.Service.Interface.Payment;
using Core.Service.Interface.Shop;
using Core.Service.Interface.Users;
using Core.Service.Services.Shop;
using Domain;
using Domain.Delivery;
using Domain.Dr;
using Domain.Shop;
using Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStore.Areas.UserPanel.Controllers;
using WebStore.Base;

namespace DrMoradi.Areas.UserPanel.Controllers
{
    [Area(AreaName.UserPanel)]
    [Authorize]
    public class ShopController : BaseController
    {
        private readonly ICart _cart;
        private readonly IOrder _order;
        private readonly IAddress _address;
        private readonly IProvince _province;
        private readonly IPayment _payment;
        private readonly ILogger<ShopController> _logger;
        private readonly IUser _user;
        private readonly IDelivery _Delivery;
        private readonly IProduct _product;
        private readonly ISms _sms;

        public ShopController(ICart cart, IAddress address, IProvince province, IPayment payment, ILogger<ShopController> logger, IOrder order, IUser user, IDelivery delivery, IProduct product, ISms sms)
        {
            _cart = cart;
            _address = address;
            _province = province;
            _payment = payment;
            _logger = logger;
            _order = order;
            _user = user;
            _Delivery = delivery;
            _product = product;
            _sms = sms;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _order.GetAllOrderByUserId(User.GetUserId()));
        }
        public async Task<IActionResult> orderDetail(int OrderId)
        {

            var order = await _order.GetOrderById(OrderId);



            return View(order);
        }
        [HttpGet]
        public async Task<IActionResult> Invoice()
        {

            var cart = await _cart.GetCartByUserId(User.GetUserId());
            return View(cart); // ویوی فاکتور که با بوت‌استرپ 5 ساختیم
        }
        [HttpGet]
        public async Task<IActionResult> UserAddress()
        {
            ViewBag.Province = new SelectList(await _province.GetAll(), "Id", "Title");
            GetAddressOfUserVM add = new GetAddressOfUserVM()
            {
                OldAddress = await _address.GetAddressOfUser(User.GetUserId()),
                NewAddress = null

            };
            return View(add); // ویوی فاکتور که با بوت‌استرپ 5 ساختیم
        }
        [HttpGet]
        public async Task<IActionResult> AloPeyk()
        {
            var obj = await _address.GetAloPeykAddressOfUser(User.GetUserId());
            return View(obj);

        }
        [HttpGet]
        public IActionResult AddAdressAloPeyk()
        {

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> AddAdressAloPeyk(ShippingAddres addres)
        {
            if (!ModelState.IsValid)
            {
                return View(addres);
            }

            addres.provinceId = 8;
            addres.UserId = User.GetUserId();
            var result = await _address.Add(addres);
            if (result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("AloPeyk");
            }
            return View(addres);

        }


        public async Task<IActionResult> GetDeliveryPrice([FromBody] AloPeykPriceRequest request)
        {

            try
            {
                // مدل درخواست برای سرویس الوپیک

                // صدا زدن سرویس الوپیک از لایه سرویس
                var priceResult = await _Delivery.GetPriceAsync(request);

                if (priceResult == null || priceResult.Status != "success" || priceResult.Object == null)
                {
                    return Json(new
                    {
                        status = "error",
                        message = priceResult?.Message ?? "Failed to get price from AloPeyk."
                    });
                }


                return Json(new
                {
                    status = "success",
                    price = priceResult.Object.Final_Price,  // قیمت نهایی
                    distance = priceResult.Object.Distance,  // فاصله (متر)
                    duration = priceResult.Object.Duration   // مدت (ثانیه)
                    // می‌تونی فیلدهای دیگر هم اضافه کنی در صورت نیاز
                });
            }
            catch (Exception ex)
            {
                // برای عیب‌یابی
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex?.Message ?? "Failed to get price from AloPeyk."
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> FinalFaktorAlopeyk(int addressId, int price)
        {
            var address = await _address.GetAddresById(addressId);
            var cart = await _cart.GetCartByUserId(User.GetUserId());

            if (address == null || price == 0)
            {
                return NotFound();
            }
            var obj = new FinalInvoiceVM()
            {
                Address = address,
                Items = cart.Items,
                SendPrice = (int)price

            };

            return View(obj);
        }
        [HttpGet]
        public async Task<IActionResult> FinalFaktor(int AddressId)
        {
            var address = await _address.GetAddresById(AddressId);
            var cart = await _cart.GetCartByUserId(User.GetUserId());
            decimal price = await _cart.CalculatePrice(User.GetUserId(), address.provinceId);
            if (address == null || price == 0)
            {
                return NotFound();
            }
            var obj = new FinalInvoiceVM()
            {
                Address = address,
                Items = cart.Items,
                SendPrice = (int)price

            };

            return View(obj); 
        }
        [HttpGet]
        public async Task<IActionResult> AddtoOrder(int AddressId,int sendprice)
        {
            DeliveryMethod method;
            decimal price = 0;
            var address = await _address.GetAddresById(AddressId);
            var cart = await _cart.GetCartByUserId(User.GetUserId());
            if (sendprice==0)
            {
                 price = await _cart.CalculatePrice(User.GetUserId(), address.provinceId);
                 method = DeliveryMethod.Post;
            }
            else
            {
                price = sendprice*10;
                method = DeliveryMethod.AloPeyk;

            }
            if (address == null || price == 0)
            {
                return NotFound();
            }

            var Order = await _order.FillOrder(cart, User.GetUserId(), AddressId, (int)price,method);
            if (Order != null)
                return RedirectToAction("StartPaymentShop", new { orderId = Order.Id });
            else
            {
                TempData[warning] = "انتقال به فاکتور نهایی با مشکل مواجه شد";
                return RedirectToAction("FinalFaktor", new { addressId = AddressId });

            }
        }

        public async Task<IActionResult> StartPaymentShop(int orderId)
        {
            _logger.LogInformation("شروع فرایند پرداخت برای فروشگاه orderId={orderId}, UserId={UserId}", orderId, User.GetUserId());
            Order? Order = null;
            if (orderId == 0)
            {
                Order = await _order.GetOrderByUserId(User.GetUserId());

            }
            else
            {
                Order = await _order.GetOrderById(orderId);
            }

            if (Order == null)
            {
                _logger.LogError("orderId با شناسه {orderId} یافت نشد. UserId={UserId}", orderId, User.GetUserId());
                TempData[Error] = "اطلاعات رژیم پیدا نشد";
                return RedirectToAction("Index");
            }
            string callbackUrl = "https://www.drmoradi-diet.com/UserPanel/Shop/verify";
            var First = await _payment.FirstRequestPayment(Order.Id, (int)Order.TotalAmount, callbackUrl, "خرید اقلام", "", User.Identity?.Name, Core.Enums.StoreType.Shop, false);
            if (First.data != null)
            {
                _logger.LogInformation("درخواست اولیه پرداخت موفق. Authority={Authority}, Amount={Amount}, UserId={UserId}", First.data.authority, Order.TotalAmount, User.GetUserId());
                return RedirectToAction("SendTOBank", new { Url = "https://zarinpal.com/pg/StartPay/" + First.data.authority });
                //return RedirectToAction("SendTOBank", new { Url = "https://sandbox.zarinpal.com/pg/StartPay/" + First.data.authority });
            }
            else
            {
                _logger.LogError("درخواست اولیه پرداخت ناموفق. OrderId={OrderID}, UserId={UserId}", Order.Id, User.GetUserId());
                TempData[Error] = "صفحه پرداخت با مشکل مواجه گردیده است";
                return RedirectToAction("Index");
            }
        }
        public IActionResult SendTOBank(string Url)
        {
            _logger.LogInformation("انتقال کاربر {UserId} به درگاه بانکی. URL={Url}", User.GetUserId(), Url);
            return Redirect(Url);
        }
        [HttpGet]
        public async Task<IActionResult> verify(string Authority, string Status)
        {
            _logger.LogInformation("بازگشت از درگاه بانکی. Authority={Authority}, Status={Status}, UserId={UserId}", Authority, Status, User.GetUserId());
            if (Status == "OK")
            {
                var Order = await _order.GetOrderByAutority(Authority);
                if (Order == null)
                {
                    _logger.LogError("هیچ Order مرتبط با Authority={Authority} پیدا نشد. UserId={UserId}", Authority, User.GetUserId());
                    TempData[Error] = "پرداخت پیدا نشد";
                    return RedirectToAction("Index");
                }
                var payevent = await _payment.VerifyPayment(authority: Authority, (int)Order.TotalAmount, StoreType.Shop, false);

                if (payevent.Error == null)
                {
                    _logger.LogInformation("پرداخت موفق. UserId={UserId}, Amount={Amount}, order={orderId}, Authority={Authority}",
                 User.GetUserId(), Order.TotalAmount, Order.Id, Authority);

                    // علامت‌گذاری سفارش به‌عنوان پرداخت شده
                    await _product.deActiveBatch(Order.OrderItems.First().ProductBatchId);

                    //if(Order.DeliveryMethod==DeliveryMethod.AloPeyk)
                    //{
                    //    await _Delivery.CreateAloPeykOrderAsync(Order, Order.ShippingAddress, Order.User);

                    //}


              

                    var myUser=await _user.GetUserByUserId(User.GetUserId());
                    if (Order.DeliveryMethod == DeliveryMethod.AloPeyk)
                    {
                        await _sms.PaymentSucessProductAloPeyk(myUser.UserName, 1466231,".", myUser.FullName, payevent.data.ref_id);
                        await _sms.AdminAlarmProduct("09128390869", 1466233, Order.Id.ToString(), myUser.FullName);
                        TempData[Success] = "  پرداخت شما با موفقیت انجام شد اقلام دو تا چهار روز کاری تحویل می گردد . :" + payevent.data.ref_id;

                    }
                    else
                    {
                        await _sms.PaymentSucessProductPost(myUser.UserName, 1466231, ".", myUser.FullName,payevent.data.ref_id);
                        await _sms.AdminAlarmProduct("09128390869", 1466233, Order.Id.ToString(), myUser.FullName);
                        TempData[Success] = "  پرداخت شما با موفقیت انجام شد اقلام چهار تا هفت روز کاری تحویل می گردد . :" + payevent.data.ref_id;

                    }

                    // نمایش خطا
                    return RedirectToAction("Index", "Shop", "UserPanel");
                    




                 
                }
                else
                {
                    _logger.LogWarning("پرداخت تایید نشد. UserId={UserId}, Amount={Amount}, Authority={Authority}",
                    User.GetUserId(), Order.TotalAmount, Authority);
                    TempData[Error] = " پرداخت با مشکل مواجه گردیده است";
                    // نمایش خطا
                    return RedirectToAction("Index", "Shop", "UserPanel");
                }
            }
            if (Status == "NOK")
            {
                _logger.LogWarning("پرداخت لغو شد یا ناموفق بود. UserId={UserId}, Authority={Authority}",
          User.GetUserId(), Authority);
                TempData[Error] = " پرداخت با مشکل مواجه گردیده است";
                // نمایش خطا
                return RedirectToAction("Index", "Shop", "UserPanel");
            }

            _logger.LogWarning("وضعیت پرداخت موفق. Status={Status}, UserId={UserId}, Authority={Authority}",
           Status, User.GetUserId(), Authority);
            TempData[Success] = " از پرداخت شما متشکریم";
            // نمایش خطا
            return RedirectToAction("Index", "Shop", "UserPanel");
        }
        [HttpPost]
        public async Task<IActionResult> AddCity(AddAdressVm model)
        {
            if (!ModelState.IsValid)
            {

                var errors = ModelState
              .Where(x => x.Value.Errors.Any())
              .ToDictionary(
                  kvp => kvp.Key,
                  kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
              );

                return Json(new { success = false, errors });
            }
            ShippingAddres obj = new ShippingAddres()
            {
                UserId = User.GetUserId(),
                AddressLine = model.AddressLine,
                PostalCode = model.PostalCode,
                provinceId = model.provinceId.Value,


            };

            var result = await _address.Add(obj);
            if (result)
            {
                return Json(new
                {
                    success = true,
                    newAddress = new
                    {
                        Id = obj.Id,
                        AddressLine = obj.AddressLine,
                        PostalCode = obj.PostalCode
                    }
                });
            }
            else
            {
                return Json(new { success = false, errors = "خطا" });
            }

        }
    }
}
