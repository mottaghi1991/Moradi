using Domain.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Sms
{
    public interface IUserOtp
    {
        public Task<bool> insertAsync(UserOtp userOtp);
        public Task<bool> AcceptCodeAsync(string PhoneNumber, string Code, DateTime EnterTime);
        public Task<bool> UseCodeAsync(string PhoneNumber, string Code);
        public Task<bool> CanTryAsync(string phoneNumber);
        public Task IncreaseTryAsync(string phoneNumber);
        public Task ResetAsync(string phoneNumber);

    }
}
