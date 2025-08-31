using Core.Dto.ViewModel.Dr;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Dr
{
    public interface IUserAnswer
    {
        Task<bool> BulkInsertUserAnswer(List<UserAnswer> userAnswers);
        Task<IEnumerable<UserAnswer>> GetAnswerOfUser(int  userId);
        Task<IEnumerable<ShowUserAnswerVM>> GetUserAnswerByUserIdAndUserDietId(int UserId, int UserDietId);
        Task<string> getNameFiel(int userid,int UserDIetId);
    }
}
