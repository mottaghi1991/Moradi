using Core.Dto.Shop.CartDto;
using Core.Extention;
using Core.Interface.Store;
using Core.Service.Interface.Shop;
using Data;
using Data.MasterInterface;
using Domain.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class CartServices : ICart
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMaster<Cart> _master;
        private readonly ICartItem _cartItem;
        private readonly IProduct _product;

        public CartServices(IMaster<Cart> master, IHttpContextAccessor httpContextAccessor, ICartItem cartItem, IProduct product)
        {
            _master = master;
            _httpContextAccessor = httpContextAccessor;
            _cartItem = cartItem;
            _product = product;
        }

      

        public async Task<bool> AddToDbCart(ClaimsPrincipal user, int productId, int quantity)
        {
            using var transaction = await _master.BeginTransactionAsync();
            try
            {
                var userId = user.GetUserId();
                var product = await _product.GetProductById(productId);
                var cart = _master.GetAllAsQueryable(a => a.UserId == userId).Include(a => a.Items).FirstOrDefault();
                //var cart = _db.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId, Items = new List<CartItem>() };
                    await _master.InsertAsync(cart);
                }
                var cartItem = cart.Items.Where(a => a.ProductId == productId).FirstOrDefault();
                //var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;

                    await _cartItem.UpdateAsync(cartItem);
                }
                else
                {
                    await _cartItem.InsertAsync(new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = productId,
                        Quantity = quantity,
                        UnitPrice = product.Price
                    });

                }
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return false;
            }
           
 
        }

        public async Task<Cart> GetCartByUserId(int UserId)
        {
            var obj = await _master.GetAllEfAsync(a => a.UserId == UserId);
               return obj.FirstOrDefault();
        }

       

        public async Task<bool> Insert(Cart cart)
        {
          var obj=await _master.InsertAsync(cart);
            return obj != null;
        }


        public async Task<bool> Update(Cart cart)
        {
            var obj = await _master.UpdateAsync(cart);
            return obj != null;
        }

        public async Task<IEnumerable<CartItemDto>> UpdateCart(int UserId)
        {
        return await  _master.GetAllAsQueryable()
                         .Include(c => c.Items)
                         .ThenInclude(i => i.Product)
                         .Where(c => c.UserId == UserId)
                         .SelectMany(c => c.Items)
                         .Select(i => new CartItemDto
                         {
                             ProductId = i.ProductId,
                             Quantity = i.Quantity,
                             Price = i.UnitPrice,
                             ProductName = i.Product.ProductName
                         })
                         .ToListAsync();
        }
    }
}
