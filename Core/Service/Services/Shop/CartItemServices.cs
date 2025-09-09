using Core.Service.Interface.Shop;
using Data;
using Data.MasterInterface;
using Domain.Shop;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Shop
{
    public class CartItemServices : ICartItem
    {
        private readonly IMaster<CartItem> _master;

        public CartItemServices(IMaster<CartItem> master)
        {
            _master = master;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int userId)
        {
            return await _master.GetAllAsQueryable()
                  .Include(ci => ci.Product)
                  .Where(ci => ci.Cart.UserId == userId)
                  .ToListAsync();
        }

        public async Task<bool> InsertAsync(CartItem cartItem)
        {
            var obj = await _master.InsertAsync(cartItem);
            return obj != null;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int productId)
        {
            var cartItem = await _master.GetAllAsQueryable()
                   .Include(ci => ci.Cart)
                   .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.ProductId == productId);

            if (cartItem == null)
                return false;

           await _master.DeleteAsync(cartItem);
            return true;
        }

        public async Task<bool> UpdateAsync(CartItem cartItem)
        {
            var obj = await _master.UpdateAsync(cartItem);
            return obj != null;
        }
    }
}
