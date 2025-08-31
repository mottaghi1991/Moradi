using Core.Interface.Sms;
using Domain.SMS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Sms
{
    public class KavehNegarSmsServices : ISms
    {
        public async Task<SmsResponse> SendSms(string mobile, int TemplateId, string code)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("316B44514A3637464F4D466B494B462B4B2B734C706264327A4237733254455443755945453337635962493D");
                var result = api.VerifyLookup( mobile, code, "ACCOUNTSendCodeVerify").Result;
                result.Messageid.ToString();
                return new SmsResponse()
                {
                    Status = 500,
                    Message = result.Message,

                };
            }
            catch (Kavenegar.Core.Exceptions.ApiException ex)
            {
                return new SmsResponse()
                {
                    Status = 500,
                    Message = ex.Message,

                };
            }
            catch (Kavenegar.Core.Exceptions.HttpException ex)
            {
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                return new SmsResponse()
                {
                    Status = 500,
                    Message = ex.Message,

                };
            }
        }

    }
}
