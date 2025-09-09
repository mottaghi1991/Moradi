using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Shop
{
    public interface ICartItem
    {
        public Task<bool> InsertAsync(CartItem cartItem);   
        public Task<bool> UpdateAsync(CartItem cartItem);
        public Task<List<CartItem>> GetCartItemsAsync(int userId);
        public Task<bool> RemoveFromCartAsync(int userId, int productId);

    }
}
