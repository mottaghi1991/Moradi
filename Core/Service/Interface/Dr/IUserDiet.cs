using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.main;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Dr
{
    public interface IUserDiet
    {
        Task<bool> UserHasDiet(int UserId, int DietId);
        Task<UserDiet> InsertUserDiet(UserDiet userDiet);
        Task<IEnumerable<UserDiet>> GetAllUserDiets();
        Task<IEnumerable<UserDiet>> GetAllDietsByUserIdAndDietId(int UserId, int DietId);
        Task<IEnumerable<ShowUserDietPanelVm>> GetAllDietsByUserId(int UserId);
        Task<Paging<ShowUserDietPanelVm>> GetAllDietsByFilter(int? userId, string paymentStatus, string fullName, string mobile,int pageNumber, int pageSize);
        Task<IEnumerable<UserDiet>> GetAllDietsByUserIdAnother(int UserId);
        Task<bool> DeleteUserDiet(UserDiet userDiet);
        Task<bool> UpdateToSend(int UserDietId);
        Task<bool> UpdateToFirstPay(int UserdietId, string Authority);
        Task<bool> UpdateToFinaltPay(UserDiet userDiet);
        Task<UserDiet> GetUserDietById(int UserDietId);
        Task<bool> InsertAnswerAsync(ShowQuestionToUserVM vm,int UserId);
       
        Task<UserDiet> GetUserDietByAuthority(string Authority);
        Task<IEnumerable<UserDiet>> GetAllParentAndChild(int userDietId);
    }
}
