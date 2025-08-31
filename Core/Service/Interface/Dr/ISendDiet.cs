using Domain.Dr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Dr
{
    public interface ISendDiet
    {
        Task<bool> InsertSendDiet(SendDiet sendDiet);
        Task<bool> UpdateSendDiet(SendDiet sendDiet);
        Task<SendDiet> GetSendDietByUserDietId(int UserDietId);
        Task<SendDiet> GetSendDietByUserDietIdAndUserId(int UserDietId,int UserId);

    }
}
