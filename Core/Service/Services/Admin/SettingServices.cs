using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Interface.Admin;
using Data.MasterInterface;
using Domain.PersonalData;
using Microsoft.Extensions.Caching.Memory;

namespace Core.Service.Services.Admin
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
                var obj=Mysetting.FirstOrDefault();
                if (obj == null)
                {
                    return new Setting()
                    {
                        Name = "علی متقی",
                        Phone = "09124790243",
                        Location = "تهران",
                        Birthday = new DateTime(1991, 1, 1),
                        Aboute = "",
                        BackgroundImage = "",
                        Email = "ali.mottaghi1991@gmail.com",
                        Instagram = "",
                        Linkedin = "",
                        Tweeter = "",
                        jobs = "Developer",
                        DeleteTime = null,
                        IsDeleted = false,
                        Id = 0
                    };
                }

                return obj;
            });
        }

        public async Task<bool> UpdateSettingAsync(Setting setting)
        {
            var res =await _master.UpdateAsync(setting);
            if (res == null)
                return false;
            _cache.Remove("SiteSetting");
            return true;
        }
    }
}
