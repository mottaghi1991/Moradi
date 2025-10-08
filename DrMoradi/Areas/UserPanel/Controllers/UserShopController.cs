using Core.Dto.Shop.CartDto;
using Core.Extention;
using Core.Interface.Sms;
using Core.Interface.Store;
using Core.Service.Interface.Shop;
using Core.Service.Services.Shop;
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

            var userId = User.GetUserId(); // متد Extension خودت برای گرفتن Id کاربر

            foreach (var item in guestCart)
            {
                var maxStock = await _product.GetStockAsync(item.ProductId);
                if (maxStock <= 0) continue;

                var exists = await _cart.GetCartItemAsync(userId, item.ProductId);
                var totalQuantity = (exists?.Quantity ?? 0) + item.Quantity;

                if (totalQuantity > maxStock)
                    totalQuantity = maxStock;

                if (exists != null)
                    await _cart.UpdateCartItemAsync(userId, item.ProductId, totalQuantity);
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
<<<<<<< Updated upstream
                    price = (c.UnitPrice/10),
=======
                    price = c.UnitPrice/10,
>>>>>>> Stashed changes
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
    }
}
