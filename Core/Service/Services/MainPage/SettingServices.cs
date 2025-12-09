using Core.Interface.Admin;
using Core.Interface.MainPage;
using Data.MasterInterface;
using Domain.Main;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.MainPage
{
    public class SettingServices : ISetting
    {
        private readonly IMemoryCache _cache;

        private readonly IMaster<Setting> _master;

        public SettingServices(IMaster<Setting> master, IMemoryCache cache)
        {
            _master = master;
            _cache = cache;
        }



        public async Task<Setting> GetSettingAsync()
        {
            return await _cache.GetOrCreateAsync<Setting>("SiteSetting", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);

                var Mysetting = await _master.GetAllEfAsync(null);
                var obj = Mysetting.FirstOrDefault();
                if (obj == null)
                {
                    var defaultSetting = new Setting()
                    {
                       
                        Address= "تهران، گیشا، بین خیابان 12 و 14، ساختمان خاتم، طبقه اول، واحد 2.",
                        FooterDescript= "دکتر معصومه مرادی - متخصص تغذیه و رژیم درمانی - نظام پزشکی ت-2542 دکترای حرفه ای تغذیه و رژیم درمانی از دانشگاه علوم پزشکی تهران مدرس دانشگاه در رشته های تغذیه و رژیم درمانی و تغذیه ورزشی فعالیت در زمینه مشاوره تغذیه و رژیم درمانی از سال 1385 فعالیتهای آموزشی و پژوهشی در حیطه تغذیه انتشار مقالات علمی بین المللی در حوزه تغذیه و رژیم شناسی عضو انجمن تغذیه ایران (اتا) عضو پیوسته انجمن علمی تغذیه ایران (اعتا) ",
                        Logo= "/FileUpload/Slider/logo-m.png",
                        MainBanner= "/FileUpload/Slider/297b48dab4e0f159911593c8ae73c5506303cbe5b31ba.jpg",
                        MainBannerAddress="",
                        Mobile= "09194820425",
                        Number1= "09194820425",
                        Number2="02188231136",
                        SiteName="دکتر معصومه مرادی",
                        MapAddress= "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d624.3594308115614!2d51.37789755897238!3d35.73173473218402!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3f8e073b3bac15f5%3A0x94ebcba82094394c!2z2K_aqdiq2LEg2YXYudi12YjZhdmHINmF2LHYp9iv24w!5e0!3m2!1sfa!2s!4v1755409212138!5m2!1sfa!2s",
                        WorkTime= "مطب: شنبه،یکشنبه و چهارشنبه ساعت 13 تا 19 - روزهای دوشنبه و پنجشنبه ساعت 10 صبح تا 15",
                        Email="",
                        
                    };
                    await _master.InsertAsync(defaultSetting);
                    return defaultSetting;
                }
              
                return obj;
            });
        }

  

        public async Task<bool> UpdateSettingAsync(Setting setting)
        {
            var res = await _master.UpdateAsync(setting);
            if (res == null)
                return false;
            _cache.Remove("SiteSetting");
            return true;
        }
    }
}
