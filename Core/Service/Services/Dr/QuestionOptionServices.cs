using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Interface.Dr;
using Data.MasterInterface;
using Domain.Dr;

namespace Core.Service.Services.Dr
{
    public class QuestionOptionServices : IQuestionOption
    {
        private readonly IMaster<QuestionOption> _master;

        public QuestionOptionServices(IMaster<QuestionOption> master)
        {
            _master = master;
        }

        public async Task<IEnumerable<QuestionOption>> GetQuestionOptionsByQuestionId(int questionId)
        {
            return await _master.GetAllEfAsync(a => a.QuestionId == questionId);
        }
    }
}
