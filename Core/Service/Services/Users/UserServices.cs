using Core.Dto.ViewModel.User;
using Core.Extention;
using Core.Service.Interface.Admin;
using Core.Service.Interface.Users;
using Data.MasterInterface;
using Domain.User;
using Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Users
{
    public class UserServices : IUser
    {
        private readonly IMaster<MyUser> _User;
        private readonly IRole _Role;

        public UserServices(IMaster<MyUser> user, IRole role)
        {
            _User = user;
            _Role = role;
        }



        public async Task<bool> ISExistUserNameAsync(string userName)
        {
            var obj = await _User.GetAllEfAsync(a => a.UserName == userName);
            return obj.Any();
        }

      public  async Task<bool> ISExistEmailAsync(string Email)
        {
            var obj =await _User.GetAllEfAsync(a => a.Email == StringTools.FixEmail(Email));
            return obj.Any();
        }

        public async Task<MyUser> AddUserAsync(MyUser user)
        {
            return await _User.InsertAsync(user);
        }

       public async Task<MyUser> LoginCheckAsync(LoginViewModel model)
        {

            var obj = await _User.GetAllEfAsync(a =>
                a.UserName == StringTools.FixEmail(model.UserName) &&
                a.PassWord == PasswordHelper.EncodePasswordMD5(model.PassWord) & a.IsActive == true);
            return obj.FirstOrDefault();
        }

        public async Task<InformationUserViewModel> GetUserInformationAsync(string userName)
        {
            var user =await GetUserByUserNameAsync(userName);
            var infomation = new InformationUserViewModel()
            {
                Email = user.Email,
                UserName = user.UserName,
                RegisterDate = user.RegisterDate,
                Wallet = 0
            };
            return infomation;
        }

        public async Task<MyUser> GetUserByUserNameAsync(string userName)
        {
            var obj = await _User.GetAllEfAsync(a => a.UserName == StringTools.FixEmail(userName));
            return obj.SingleOrDefault();
        }

        public int BalanceUserWallet(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<UserPanelViewModel> GetUserPanelAsync(string userName)
        {
            var obj = await _User.GetAllEfAsync(a => a.UserName == StringTools.FixEmail(userName));
            return obj.Select(u => new UserPanelViewModel()
            {
                Name = u.Email,
                Image = u.UserAvatar
            }).Single();
        }

        public async Task<bool> IsActiveCodeAsync(string code)
        {
            var obj = await _User.GetAllEfAsync(a => a.ActiveCode == code);
            return obj.Any();
        }

        public async Task<MyUser> GetUserByActiveCodeAsync(string code)
        {
            var obj = await _User.GetAllEfAsync(a => a.ActiveCode == code);
            return obj.FirstOrDefault();
        }

        public async Task<MyUser> UpdateAsync(MyUser user)
        {
            return await _User.UpdateAsync(user);
        }

        public async Task<IEnumerable<ShowUserBrifViewModel>> GetPaggingUserAsync(int Page, int pagesize)
        {
            var obj =await _User.GetPagingAsync(Page, pagesize);
            return obj.Select(a => new ShowUserBrifViewModel() { Email = a.Email, UserName = a.UserName, UserId = a.ItUserId });
        }

        public async Task<IEnumerable<ShowUserBrifViewModel>> GetAllAdminAsync()
        {
            var obj = await _User.GetAllEfAsync(a => a.IsAdmin == true);
            return obj.Select(a => new ShowUserBrifViewModel() { Email = a.Email, UserName = a.UserName, UserId = a.ItUserId });
        }
        public async Task<MyUser> GetUserByUserId(int userId)
        {
            var obj = await _User.GetAllEfAsync(a => a.ItUserId == userId);
            return obj.FirstOrDefault();
        }

        public async Task<MyUser> GetOrCreateUser(string phoneNumber)
        {
            var user =await GetUserByUserNameAsync(phoneNumber);
            if (user != null)
                return user;

            var newUser = new MyUser
            {
                UserName = phoneNumber,
                Email = "",
                IsActive = true,
                PassWord = PasswordHelper.EncodePasswordMD5(CodeGenerator.Generate()),
                RegisterDate = DateTime.Now,
                UserAvatar = "default.jpg",
                IsAdmin = false,
                ActiveCode = StringTools.GenerateUniqeCode()
            };

            var result =await AddUserAsync(newUser);
          await  _Role.UserRoleInsertAsync(new UserRole { RoleId = 2, UserId = result.ItUserId });

            return result;
        }

        public  async Task SignIn(HttpContext context, MyUser user)
        {
            if (user.FullName == null)
            {
                user.FullName = "";
            }
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.ItUserId.ToString()),
            
            new Claim(ClaimTypes.Name, user.FullName)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties { IsPersistent = true };
          await  context.SignInAsync(principal, properties);
        }
    }
}
