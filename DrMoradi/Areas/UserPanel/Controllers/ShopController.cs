using Core.Dto.Shop.Address;
using Core.Enums;
using Core.Extention;
using Core.Interface.Sms;
using Core.Service.Interface.Payment;
using Core.Service.Interface.Shop;
using Core.Service.Interface.Users;
using Core.Service.Services.Shop;
using Domain.Shop;
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

        public ShopController(ICart cart, IAddress address, IProvince province, IPayment payment, ILogger<ShopController> logger, IOrder order, IUser user)
        {
            _cart = cart;
            _address = address;
            _province = province;
            _payment = payment;
            _logger = logger;
            _order = order;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _order.GetAllOrderByUserId(User.GetUserId()));
        }
        public async Task<IActionResult> orderDetail(int OrderId)
        {

            var order =await _order.GetOrderById(OrderId);
            
            

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
          var obj = await _address.GetAddressOfUser(User.GetUserId());
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

           var result=await _address.Add(addres);
           if (result)
           {
               TempData[Success] = SuccessMessage;
               return RedirectToAction("AloPeyk");
           }
           return View(addres);

        }
        [HttpGet]
        public async Task<IActionResult> FinalFaktor(int AddressId)
        {
            var address = await _address.GetAddresById(AddressId);
            var cart = await _cart.GetCartByUserId(User.GetUserId());
            decimal price =await _cart.CalculatePrice(User.GetUserId(), address.provinceId);
            if (address==null || price==0)
            {
                return NotFound();
            }
            var obj = new FinalInvoiceVM()
            {
                Address = address,
                Items=cart.Items,
                SendPrice=(int) price

            };

            return View(obj); // ویوی فاکتور که با بوت‌استرپ 5 ساختیم
        }
        [HttpGet]
        public async Task<IActionResult> AddtoOrder(int AddressId)
        {
            var address = await _address.GetAddresById(AddressId);
            var cart = await _cart.GetCartByUserId(User.GetUserId());
            decimal price = await _cart.CalculatePrice(User.GetUserId(), address.provinceId);
            if (address == null || price == 0)
            {
                return NotFound();
            }
          
          var merg= await _order.FillOrder(cart, User.GetUserId(),AddressId,(int)price);
            if(merg)
                return RedirectToAction("StartPaymentShop");
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
          if (orderId==0)
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
            string callbackUrl = $"{Request.Scheme}://{Request.Host}/UserPanel/Shop/verify";
            var First = await _payment.FirstRequestPayment(Order.Id, (int)Order.TotalAmount, callbackUrl,"خرید اقلام","" , User.Identity?.Name,Core.Enums.StoreType.Shop);
            if (First.data != null)
            {
                _logger.LogInformation("درخواست اولیه پرداخت موفق. Authority={Authority}, Amount={Amount}, UserId={UserId}", First.data.authority, Order.TotalAmount, User.GetUserId());
                //return RedirectToAction("SendTOBank", new { Url = "https://zarinpal.com/pg/StartPay/" + First.data.authority });
                return RedirectToAction("SendTOBank", new { Url = "https://sandbox.zarinpal.com/pg/StartPay/" + First.data.authority });
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
                var payevent = await _payment.VerifyPayment(authority: Authority, (int)Order.TotalAmount,StoreType.Shop);
                var myuser = await _user.GetUserByUserId(User.GetUserId());
                if (payevent.Error == null)
                {
                    _logger.LogInformation("پرداخت موفق. UserId={UserId}, Amount={Amount}, order={orderId}, Authority={Authority}",
                 User.GetUserId(), Order.TotalAmount, Order.Id, Authority);
                    //await _sms.PaymentSucess(myuser.UserName, 502848, payevent.data.ref_id);
                    //await _sms.AdminAlarm("09128390869", 502847, userdiet.Id.ToString(), myuser.FullName);
                    TempData[Success] = " پرداخت شما با موفقیت انجام شد :" + payevent.data.ref_id;
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
             ShippingAddres obj=new ShippingAddres()
             {
                 UserId = User.GetUserId(),
                 AddressLine=model.AddressLine,
                 PostalCode=model.PostalCode,
                 provinceId=model.provinceId.Value,
                 
                 
             };
          
            var result = await _address.Add(obj);
            if (result)
            {
                return Json(new
                {
                    success = true,
                    newAddress = new
                    {
                        Id = obj.provinceId,
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
