using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dr;

namespace Core.Service.Interface.Dr
{
    public interface IQuestionOption
    {
        Task<IEnumerable<QuestionOption>> GetQuestionOptionsByQuestionId(int questionId);
    }
}
