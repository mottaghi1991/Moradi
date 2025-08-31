using Core.Service.Interface.Dr;
using Core.Service.Interface.Payment;
using Domain;
using Domain.Dr;
using Domain.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Payment
{
    public class PaymentServices : IPayment
    {
        private readonly HttpClient _httpClient;
        private readonly IUserDiet _userDiet;
        private readonly string _merchantId = "bb48b37a-7d42-4452-b65f-ccf8aa7a4e21";

        public PaymentServices(HttpClient httpClient, IUserDiet userDiet)
        {
            _httpClient = httpClient;
            _userDiet = userDiet;
        }

        public async Task<PaymentFirstResponse> FirstRequestPayment(int userDietId, int amount, string callbackUrl, string description, string email, string mobile)
        {
            var requestData = new PaymentRequest()
            {
                Metadata = new Metadata()
                {
                    Email = email,
                    Mobile = mobile
                },
                merchant_id = _merchantId,
                amount = amount,
                callback_url = callbackUrl,
                description = description,


            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            //var response = await _httpClient.PostAsync("https://api.zarinpal.com/pg/v4/payment/request.json", content);
            var response = await _httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/request.json", content);
            var result = await response.Content.ReadAsStringAsync();

            dynamic FInalresult = JsonConvert.DeserializeObject<dynamic>(result);
            if (FInalresult.data.code != null)
            {
                await _userDiet.UpdateToFirstPay(userDietId, (string)FInalresult.data.authority);
                return new PaymentFirstResponse
                {
                    data = new PaymentFirstResponse.Data
                    {
                        authority= FInalresult.data.authority
                    }

                };
            }
            else
            {

                return new PaymentFirstResponse
                {
                    Error = new PaymentFirstResponse.Errors
                    {
                        message = FInalresult.errors.message,
                        code = FInalresult.errors.code,

                    },

                };
            }

            throw new Exception("خطا در ارتباط با درگاه پرداخت");
        }

        public async Task<bool> VerifyPayment(string authority, int amount)
        {
            var requestData = new
            {
                merchant_id = _merchantId,
                authority = authority,
                amount = amount // 🔥 باید Amount صحیح اینجا باشه (مثلاً از دیتابیس بخونی)
            };
            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //var response = await _httpClient.PostAsync("https://api.zarinpal.com/pg/v4/payment/verify.json", content);
            var response = await _httpClient.PostAsync("https://sandbox.zarinpal.com/pg/v4/payment/verify.json", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<dynamic>(responseContent);
            PaymentFinalResponse final = new PaymentFinalResponse();
            if (result.data.code == 100 || result.data.code == 101)
            {
                var userdiet = await _userDiet.GetUserDietByAuthority(authority);
                userdiet.PaymentRefId =(string) result.data.ref_id;
                userdiet.PaymentDate = DateTime.UtcNow;
                userdiet.Status = UserDietstatus.Pay;
                var updateresult = await _userDiet.UpdateToFinaltPay(userdiet);
                if (updateresult)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
