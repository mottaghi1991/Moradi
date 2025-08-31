using Core.Service.Interface.Dr;
using Data.MasterInterface;
using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Service.Services.Dr
{
    public class DietServices : IDiet
    {
        private readonly IMaster<Diet> _master;
        private readonly IQuestion _question;

        public DietServices(IMaster<Diet> master, IQuestion question)
        {
            _master = master;
            _question = question;
        }

        public async Task<IEnumerable<Diet>> GetAllAsync()
        {
        return await _master.GetAllEfAsync();
        }

        public async Task<IEnumerable<Diet>> GetAllByActiveAsync(bool active)
        {
            return await _master.GetAllEfAsync(a => a.Status == active);
        }

        public async Task<Diet> GetDietById(int DietId)
        {
            var obj = await _master.GetAllEfAsync(a => a.Id == DietId);
            return obj.FirstOrDefault();
        }

        public async Task<IEnumerable<Diet>> GetDietByIds(IEnumerable<int> DietIds)
        {
            var obj = await _master.GetAllEfAsync(d => DietIds.Contains(d.Id));
            return obj;
        }

        public async Task<bool> InsertAsync(Diet diet)
        {
            var result=await _master.InsertAsync(diet);
          return  result!=null;
        }

        public async Task<bool> UpdateAsync(Diet diet)
        {
            var result = await _master.UpdateAsync(diet);
            return result != null;
        }

        public async Task<bool> UpdateDietQuestionsAsync(int DietId, List<int> QuestionList, bool FirstForm = false)
        {
            using var transaction=await _master.BeginTransactionAsync();
            try
            {
                var deletedata = await _question.GetDietQuestionByDietIdAsync(DietId, FirstForm);
                if (deletedata.Any())
                {

                    if (!await _question.BulkDeleteDietQuestion(deletedata))
                    {
                        await transaction.RollbackAsync();
                        return false;
            
                    }
                    

                }
                List<DietQuestion> list = new List<DietQuestion>();
                foreach (int i in QuestionList)
                {
                    list.Add(new DietQuestion()
                    {
                        DietId = DietId,
                        QuestionId = i,
                    });
                }
                var obj = await _question.BulkInsertDietQuestion(list);
                if (obj)
                {
                  await  transaction.CommitAsync();
                    return true;

                }
                else
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false ;
            }
        }
    }
}
