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
    public class SendDietServices : ISendDiet
    {
        private readonly IMaster<SendDiet> _master;

        public SendDietServices(IMaster<SendDiet> master)
        {
            _master = master;
        }

        public async Task<SendDiet> GetSendDietByUserDietId(int UserDietId)
        {
            var obj = await _master.GetAllEfAsync(a => a.UserDietId == UserDietId);
            return obj.FirstOrDefault();
        }

        public async Task<SendDiet> GetSendDietByUserDietIdAndUserId(int UserDietId, int UserId)
        {
            var obj =await _master.GetAllEfAsync(a => a.UserDietId == UserDietId && a.userDiet.UserId == UserId&&a.userDiet.Status==Domain.UserDietstatus.send);
            return obj.FirstOrDefault();
        }

        public async Task<bool> InsertSendDiet(SendDiet sendDiet)
        {
            var obj = await _master.InsertAsync(sendDiet);
          return obj != null;
        }

        public async Task<bool> UpdateSendDiet(SendDiet sendDiet)
        {
            var obj = await _master.UpdateAsync(sendDiet);
            return obj!=null ;
        }
    }
}
