using Core.Dto.ViewModel.Dr.QuestionFolder;
using Data.Migrations;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Dr
{
    public interface IQuestion
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<IEnumerable<DietQuestion>> GetDietQuestionByDietIdAsync(int DietId, bool FirstForm);
        Task<IEnumerable<Question>> GetQuestionByDietIdAsync(int DietId, bool FirstForm);
        Task<bool> BulkDeleteDietQuestion(IEnumerable<DietQuestion> dietquestions);
        Task<bool> BulkInsertDietQuestion(IEnumerable<DietQuestion> dietquestions);
        Task<IEnumerable<QuestionWithUserAnswerVm>> GetQuestionWithAnswerOfUser(int UserId);

    }
}
