using Domain.SMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Core.Interface.Sms;


namespace Core.Services.Sms
{
    public class SmsIRServices : ISms
    {
        public Task<SmsResponse> AdminAlarm(string mobile, int TemplateId, string UserDietId, string UserName)
        {
            throw new NotImplementedException();
        }

        public Task<SmsResponse> AdminAlarmProduct(string mobile, int TemplateId, string OrderId, string UserName)
        {
            throw new NotImplementedException();
        }

        public Task<SmsResponse> PaymentSucess(string mobile, int TemplateId, string code)
        {
            throw new NotImplementedException();
        }

        public Task<SmsResponse> PaymentSucessProductAloPeyk(string mobile, int TemplateId, string code, string UserName, string RefId)
        {
            throw new NotImplementedException();
        }

        public Task<SmsResponse> PaymentSucessProductPost(string mobile, int TemplateId, string code, string UserName, string RefId)
        {
            throw new NotImplementedException();
        }

        public Task<SmsResponse> ProductSend(string mobile, int TemplateId, string code)
        {
            throw new NotImplementedException();
        }

        //private readonly HttpClient _httpClient;

        //public SmsIRServices(HttpClient httpClient)
        //{
        //    _httpClient = httpClient;
        //}


        //public  SmsResponse SendSms(string mobile,int TemplateId, string code)
        //{
        //    try
        //    {
        //        SmsIr smsIr = new SmsIr("AbX6GAvvsEEQCRMHjYerK8zDajLBEC3TyC3um086qi0VTLZJ");

        //        var bulkSendResult =  smsIr.VerifySend(mobile, TemplateId, new VerifySendParameter[]
        //        {
        //new("CODE", code) // یا new("Code", code) بسته به قالب
        //        });

        //        var res = new SmsResponse()
        //        {
        //            Status = bulkSendResult.Status,
        //            Message = bulkSendResult.Message,
        //            Data = new SmsResponseData
        //            {
        //                MessageId = bulkSendResult.Data?.MessageId ?? 0,
        //                Cost = bulkSendResult.Data?.Cost ?? 0,
        //            }
        //        };

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        // ثبت یا نمایش خطا برای دیباگ
        //        Console.WriteLine("خطا هنگام ارسال پیامک: " + ex.Message);
        //        throw; // اگر می‌خواهی مجدداً خطا پرتاب شود
        //    }

        //}
        public Task<SmsResponse> SendSms(string mobile, int TemplateId, string code)
        {
            throw new NotImplementedException();
        }

        public Task<SmsResponse> UserAlarm(string mobile, int TemplateId, string code)
        {
            throw new NotImplementedException();
        }
    }

}

