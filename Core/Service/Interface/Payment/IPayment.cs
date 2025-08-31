using Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Payment
{
    public interface IPayment
    {
        public Task<PaymentFirstResponse> FirstRequestPayment(int userDietId, int amount, string callbackUrl, string description, string email, string mobile);
        public Task<bool> VerifyPayment(string authority,int amount);
    }
}
