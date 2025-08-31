using Core.Dto.ViewModel.User;
using Domain;
using Domain.User;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Core.Service.Interface.Users
{
    public interface IUser
    {
        Task<bool> ISExistUserNameAsync(string userName);
        Task<bool> ISExistEmailAsync(string Email);

        Task<MyUser> LoginCheckAsync(LoginViewModel model);
        Task<InformationUserViewModel> GetUserInformationAsync(string userName);

        Task<MyUser> GetUserByUserNameAsync(string userName);

        int BalanceUserWallet(string userName);
        Task<UserPanelViewModel> GetUserPanelAsync(string userName);
        Task<bool> IsActiveCodeAsync(string code);
        Task<MyUser> GetUserByActiveCodeAsync(string code);
        Task<MyUser> UpdateAsync(MyUser user);
        Task<MyUser> AddUserAsync(MyUser user);
        Task<IEnumerable<ShowUserBrifViewModel>> GetPaggingUserAsync(int Page, int pagesize);
        Task<IEnumerable<ShowUserBrifViewModel>> GetAllAdminAsync();
        Task<MyUser> GetUserByUserId(int userId);
        Task<MyUser> GetOrCreateUser(string phoneNumber);
        Task SignIn(HttpContext context, MyUser user);

    }
}