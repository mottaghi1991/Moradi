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
        public Task<SmsResponse> PaymentSucess(string mobile, int TemplateId, string code);
        public Task<SmsResponse> AdminAlarm(string mobile, int TemplateId, string UserDietId,string UserName);
        public Task<SmsResponse> AdminAlarmProduct(string mobile, int TemplateId, string OrderId, string UserName);
        public Task<SmsResponse> UserAlarm(string mobile, int TemplateId, string code);

        public Task<SmsResponse> PaymentSucessProductPost(string mobile, int TemplateId, string code,string UserName,string RefId);
        public Task<SmsResponse> PaymentSucessProductAloPeyk(string mobile, int TemplateId, string code,string UserName, string RefId);
         Task<SmsResponse> ProductSend(string mobile, int TemplateId, string code);
    }
}
