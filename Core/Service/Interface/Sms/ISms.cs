using Domain.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Sms
{
    public interface ISms
    {
        public Task<SmsResponse> SendSms(string mobile, int TemplateId, string code);
    }
}
