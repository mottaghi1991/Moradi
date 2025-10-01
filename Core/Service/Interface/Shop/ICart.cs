using Azure.Identity;
using Core.Dto.Shop.CartDto;
using Domain.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Shop
{
    public interface ICart
    {


      
        public Task<bool> AddToDbCart(ClaimsPrincipal user, int productId, int quantity);
 
        public Task<IEnumerable<CartItemDto>> UpdateCart(int UserId);

        public Task<Cart> GetCartByUserId(int UserId);
        public Task<bool> Insert(Cart cart);
        public Task<bool> Update(Cart cart);
       public Task<CartItem> GetCartItemAsync(int UserId, int productId);
        public Task UpdateCartItemAsync(int userId, int productId, int quantity);
        Task<int> CalculatePrice(int UserId,int proviceId);
        Task<bool> RemoveUserCart(int UserId);


    }
}
