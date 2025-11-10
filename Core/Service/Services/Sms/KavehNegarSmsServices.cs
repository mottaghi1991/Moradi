using Core.Interface.Sms;
using Domain.Dr;
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
        private string _Api = "316B44514A3637464F4D466B494B462B4B2B734C706264327A4237733254455443755945453337635962493D";
        public async Task<SmsResponse> SendSms(string mobile, int TemplateId, string code)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
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
        public async Task<SmsResponse> PaymentSucess(string mobile, int TemplateId, string code)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
                var result = api.VerifyLookup(mobile, code, "PAYMENTSUCCESS").Result;
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

        public async Task<SmsResponse> AdminAlarm(string mobile, int TemplateId, string UserDietId, string UserName)
        {

            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
                var result =await api.VerifyLookup(mobile, UserDietId,"","", UserName,"","PAYMENTSUCCESSADMIN",Kavenegar.Core.Models.Enums.VerifyLookupType.Sms);
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

        public async Task<SmsResponse> UserAlarm(string mobile, int TemplateId, string code)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
                var result =await api.VerifyLookup(mobile,".","","",code,"SendFile");
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

        public async Task<SmsResponse> PaymentSucessProductPost(string mobile, int TemplateId, string code,string UserName, string RefId)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
                var result =await api.VerifyLookup(mobile, code,RefId, "", UserName, "UserProductAccepPost", Kavenegar.Core.Models.Enums.VerifyLookupType.Sms);
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

        public async Task<SmsResponse> PaymentSucessProductAloPeyk(string mobile, int TemplateId, string code, string UserName, string RefId)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
                var result = await api.VerifyLookup(mobile, code, RefId, "", UserName, "UserProductAccepAloPeyk", Kavenegar.Core.Models.Enums.VerifyLookupType.Sms);
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

        public async Task<SmsResponse> AdminAlarmProduct(string mobile, int TemplateId, string OrderId, string UserName)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
                var result = await api.VerifyLookup(mobile, OrderId, "", "", UserName, "", "AdminProductAccept", Kavenegar.Core.Models.Enums.VerifyLookupType.Sms);
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

        public async Task<SmsResponse> ProductSend(string mobile, int TemplateId, string code)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_Api);
                var result =await api.VerifyLookup(mobile, code, "ProductSend");
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
