using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Dr
{
    public interface IDiet
    {
        Task<IEnumerable<Diet>> GetAllAsync();
        Task<IEnumerable<Diet>> GetAllByActiveAsync(bool active);
        Task<Diet> GetDietById(int DietId);
        Task<IEnumerable<Diet>> GetDietByIds(IEnumerable<int> DietIds);
        Task<bool> InsertAsync(Diet diet);
        Task<bool> UpdateAsync(Diet diet);
        Task<bool> UpdateDietQuestionsAsync(int DietId, List<int> QuestionList, bool FirstForm = false);
    }
}
