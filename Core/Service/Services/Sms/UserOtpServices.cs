using Core.Interface.Sms;
using Data.MasterInterface;
using Domain.SMS;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Sms
{
    public class UserOtpServices : IUserOtp
    {
        private readonly IMaster<UserOtp> _master;
        private readonly IMemoryCache _cache;

        public UserOtpServices(IMaster<UserOtp> master, IMemoryCache cache)
        {
            _master = master;
            _cache = cache;
        }

        public async Task<bool> AcceptCodeAsync(string PhoneNumber, string Code, DateTime EnterTime)
        {
            var obj = await _master.GetAllEfAsync();
              var result=obj
                .Where(o => o.PhoneNumber == PhoneNumber && o.Code == Code && !o.IsUsed && o.ExpireAt > EnterTime)
         .OrderByDescending(o => o.CreateAt)
         .FirstOrDefault();
            if (obj != null)
                return true;
            return false;
        }

        public async Task<bool> CanTryAsync(string phoneNumber)
        {
            var key = $"User_Try_{phoneNumber}";
            int tryCount = _cache.GetOrCreate(key, e => {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return 0;
            });
            return tryCount < 5;
        }

        public async Task IncreaseTryAsync(string phoneNumber)
        {
            var key = $"User_Try_{phoneNumber}";
            int tryCount = _cache.GetOrCreate(key, _ => 0) + 1;
            _cache.Set(key, tryCount, TimeSpan.FromMinutes(10));
        }

        public async Task<bool> insertAsync(UserOtp userOtp)
        {
            var obj =await _master.InsertAsync(userOtp);
            if (obj == null)
                return false;

            return true;
        }

        public async Task ResetAsync(string phoneNumber)
        {
            _cache.Remove($"User_Try_{phoneNumber}");
        }

        public async Task<bool> UseCodeAsync(string PhoneNumber, string Code)
        {
            var now = DateTime.UtcNow;
            var obj = await _master.GetAllEfAsync(a => a.PhoneNumber == PhoneNumber && a.Code == Code&& now <= a.ExpireAt&&a.IsUsed==false);
               var result=obj.FirstOrDefault();
            if (result == null)
                return false;
            result.IsUsed = true;
            var Final =await _master.UpdateAsync(result);
            if (Final == null)
                return false;
            return true;
        }
    }
}
