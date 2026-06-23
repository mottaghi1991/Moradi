using Core.Dto.Shop.CartDto;
using Core.Extention;
using Core.Interface.Sms;
using Core.Interface.Store;
using Core.Service.Interface.Shop;
using Core.Service.Services.Shop;
using Core.Service.Services.Users;
using Domain.Shop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.UserPanel.Controllers
{
    [Area(AreaName.UserPanel)]
    //[AllowAnonymous]
    [Authorize]
    public class UserShopController : BaseController
    {
        private readonly ICart _cart;
        private readonly ICartItem _cartItem;
        private readonly ISms _sms;
        private readonly IProduct _product;
        public UserShopController(ICart cart, ICartItem cartItem, ISms sms, IProduct product)
        {
            _cart = cart;
            _cartItem = cartItem;
            _sms = sms;
            _product = product;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CartItemDto model)
        {
            // بررسی ورودی
            if (model == null || model.ProductId <= 0 || model.Quantity <= 0)
            {
                return BadRequest("اطلاعات نامعتبر");
            }

            var productVm = await _product.GetShowProductDetailVmByProductId(model.ProductId);
            if (productVm == null || productVm.Stock <= 0)
                return BadRequest("این محصول در حال حاضر موجود نیست.");

            // 🔹 اطمینان از اینکه افزودن درخواست‌شده بیش از موجودی نباشد
            if (model.Quantity > productVm.Stock)
                return BadRequest($"حداکثر موجودی این محصول {productVm.Stock} عدد است.");

            try
            {
             var resut=   await _cart.AddToDbCart(User,model.ProductId,model.Quantity);
                if(!resut)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                // گرفتن نسخه به‌روز سبد
                List<CartItemDto> updatedCart;

               
                    var userId = User.GetUserId();
                    var obj = await _cart.UpdateCart(userId);
                    updatedCart = obj.ToList();
               

                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> MergeCart([FromBody] List<CartItemDto> guestCart)
        {
            if (guestCart == null || guestCart.Count == 0)
                return BadRequest(new { success = false, message = "سبد مهمان خالی است" });

        

            foreach (var item in guestCart)
            {
                var activeBatch = await _product.GetActiveBatchForProduct(item.ProductId);
                if (activeBatch == null)
                    continue; // یعنی هیچ Batch فعالی وجود نداره
                // محاسبه موجودی واقعی کالا از سرویس گزارش
                var report = await _product.GetBatchUsageAsync(activeBatch.Id);
                if (report == null || report.RemainingCount <= 0)
                    continue;
                var maxStock = report.RemainingCount; // موجودی واقعی قابل فروش
                // 👇 بررسی وجود محصول در سبد فعلی کاربر
                var exists = await _cart.GetCartItemAsync(User.GetUserId(), item.ProductId);
                var totalQuantity = (exists?.Quantity ?? 0) + item.Quantity;
                // 🚫 اگر از موجودی Batch رد شد، محدودش کن
                if (totalQuantity > maxStock)
                    totalQuantity = maxStock;
                // اگر کل جمع هم هنوز صفر بود، نادیده بگیر (یعنی دیگه موجودی نداره)
                if (totalQuantity <= 0)
                    continue;
                if (exists != null)
                    await _cart.UpdateCartItemAsync(User.GetUserId(), item.ProductId, totalQuantity);
                else
                    await _cart.AddToDbCart(User, item.ProductId, totalQuantity);
            }

            return Ok(new { success = true, message = "سبد مهمان با موفقیت منتقل شد" });
        }

        [HttpPost()]
        public async Task<IActionResult> Remove(int productId)
        {
            try
            {
                // پیدا کردن کاربر
                var userId =User.GetUserId();

                // حذف محصول از سبد کاربر
                var result = await _cartItem.RemoveFromCartAsync(userId, productId);
                if (!result)
                    return BadRequest(new { message = "محصول پیدا نشد یا حذف انجام نشد." });

                // گرفتن سبد جدید از دیتابیس
                var cartItems = await _cartItem.GetCartItemsAsync(userId);

                // برگرداندن به فرمت سمت کلاینت
                var mapped = cartItems.Select(c => new
                {
                    productId = c.ProductId,
                    productName = c.Product.ProductName,
                    price = c.UnitPrice,
                    quantity = c.Quantity
                });

                return Ok(mapped);
            }
            catch (Exception ex)
            {
                // بهتره این رو لاگ هم کنی
                return StatusCode(500, new { message = "خطای داخلی سرور", detail = ex.Message });
            }
        }
        [HttpGet()]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = User.GetUserId();

                var cartItems = await _cartItem.GetCartItemsAsync(userId);

                var mapped = cartItems.Select(c => new
                {
                    productId = c.ProductId,
                    productName = c.Product.ProductName,
                    price = c.Product.ProductBatches.FirstOrDefault(a=>a.IsActive==true)?.Price/10,
                    quantity = c.Quantity
                });

                return Ok(mapped);
            }
            catch (Exception ex)
            {
                // بهتره لاگ بشه
                return StatusCode(500, new { message = "خطای داخلی سرور", detail = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartItemDto model)
        {
            if (model == null || model.ProductId <= 0 || model.Quantity <= 0)
                return Json(new { success = false, message = "درخواست نامعتبر است." });

         

            // پیدا کردن سبد فعال فعلی کاربر
            var cart = await _cart.GetCartByUserId(User.GetUserId());
            

            if (cart == null)
                return Json(new { success = false, message = "سبد خرید فعال یافت نشد." });

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == model.ProductId);
            if (cartItem == null)
                return Json(new { success = false, message = "این محصول در سبد شما نیست." });

            // پیدا کردن Batch فعال مرتبط (FIFO)
            var activeBatch = await _product.GetActiveBatchForProduct(model.ProductId);
            

            if (activeBatch == null)
                return Json(new { success = false, message = "هیچ سری خرید فعالی برای این محصول وجود ندارد." });

            // گرفتن اطلاعات موجودی واقعی از سرویس گزارش Batch
            var batchReport = await _product.GetBatchUsageAsync(activeBatch.Id);
            var remaining = batchReport?.RemainingCount ?? 0;

            if (remaining <= 0)
                return Json(new { success = false, message = "موجودی کالا به پایان رسیده است." });

            // تعداد درخواستی نباید از موجودی واقعی بیشتر باشد
            if (model.Quantity > remaining)
            {
                // اصلاح حد بالا
                return Json(new
                {
                    success = false,
                    message = $"حداکثر تعداد قابل سفارش: {remaining}"
                });
            }

            // بروزرسانی Quantity
            cartItem.Quantity = model.Quantity;
            cartItem.ProductBatchId = activeBatch.Id;
            await _cartItem.UpdateAsync(cartItem);
          

            return Json(new { success = true, message = "تعداد کالا با موفقیت بروزرسانی شد." });
        }

    }
}
