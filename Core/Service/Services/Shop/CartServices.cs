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
        private readonly IProvince _province;

        public CartServices(IMaster<Cart> master, IHttpContextAccessor httpContextAccessor, ICartItem cartItem, IProduct product, IProvince province)
        {
            _master = master;
            _httpContextAccessor = httpContextAccessor;
            _cartItem = cartItem;
            _product = product;
            _province = province;
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

        public async Task<int> CalculatePrice(int UserId, int proviceId)
        {
            var cart =await GetCartByUserId(UserId);
            var itemWeights = cart.Items.Sum(a => a.Product.Weight);
          return await _province.PriceValue(proviceId, itemWeights);
        }

        public async Task<Cart> GetCartByUserId(int UserId)
        {
            var obj = await _master.GetAllAsQueryable().Include(a => a.Items).ThenInclude(i => i.Product)
             .Where(c => c.UserId == UserId).ToListAsync();
               return obj.FirstOrDefault();
        }

        public async Task<CartItem> GetCartItemAsync(int UserId, int productId)
        {
            var obj = await _master.GetAllAsQueryable().Include(a=>a.Items).ThenInclude(i => i.Product)
             .Where(c => c.UserId == UserId).SelectMany(c => c.Items)
        .FirstOrDefaultAsync(i => i.ProductId == productId);
            return obj;
        }

        public async Task<bool> Insert(Cart cart)
        {
          var obj=await _master.InsertAsync(cart);
            return obj != null;
        }

        public async Task<bool> RemoveUserCart(int UserId)
        {
            var usercart =await GetCartByUserId(UserId);
         return  await _master.DeleteAsync(usercart);

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

        public async Task UpdateCartItemAsync(int userId, int productId, int quantity)
        {
            var cart = await _master.GetAllEfAsync();
       var cartItem = cart.Where(c => c.UserId == userId)
        .SelectMany(c => c.Items)
        .FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _cartItem.UpdateAsync(cartItem);
            }
        }
    }
}
