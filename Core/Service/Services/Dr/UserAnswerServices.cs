using Core.Dto.ViewModel.Dr;
using Core.Service.Interface.Dr;
using Data.MasterInterface;
using Domain.Dr;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Dr
{
    public class UserAnswerServices : IUserAnswer
    {
        private readonly IMaster<UserAnswer> _master;

        public UserAnswerServices(IMaster<UserAnswer> master)
        {
            _master = master;
        }

        public async Task<bool> BulkInsertUserAnswer(List<UserAnswer> userAnswers)
        {
            return await _master.BulkeInsertAsync(userAnswers);
        }

        public async Task<IEnumerable<UserAnswer>> GetAnswerOfUser(int userId)
        {
            return await _master.GetAllEfAsync(a => a.UserId == userId);
        }

        public async Task<string> getNameFiel(int userid, int UserDIetId)
        {
            var obj =await _master.GetAllEfAsync(a => a.UserId == userid && a.UserDietId == UserDIetId && a.QuestionId == 1);
            return obj.FirstOrDefault().Answer;
        }

        public async Task<IEnumerable<ShowUserAnswerVM>> GetUserAnswerByUserIdAndUserDietId(int UserId, int UserDietId)
        {
            var result =await _master.GetAllAsQueryable().Include(ua => ua.question)
          
          .Where(ua => ua.UserId == UserId && ua.UserDietId == UserDietId).Select(a=>new ShowUserAnswerVM
          {
              Answer=a.Answer,
              QuestionText=a.question.Name,
              UserDietRecordId=a.UserDietId,
              DietId=a.DietId
          })
          .ToListAsync();
            return result;
        }
    }
}
