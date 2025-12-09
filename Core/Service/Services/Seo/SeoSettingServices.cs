using Core.Service.Interface.Seo;
using Data.MasterInterface;
using Domain.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.Seo
{
    public class SeoSettingServices : ISeoSetting
    {
        private readonly IMaster<SeoData> _master;

        public SeoSettingServices(IMaster<SeoData> master)
        {
            _master = master;
        }

        public async Task<SeoData> GetPublicData()
        {
            var obj = await _master.GetAllEfAsync();
            return obj.FirstOrDefault();
        }

        public async Task<SeoData> GetSeoDataByURL(string Url)
        {
            var obj = await _master.GetAllEfAsync();
            return obj.FirstOrDefault(a => a.EntityType == Url);
        }

        public async Task<bool> Insert(SeoData seoData)
        {
            var obj = await _master.InsertAsync(seoData);
            if (obj != null)
                return true;
            return false;
        }

        public async Task<bool> update(SeoData seoData)
        {
            var obj = await _master.UpdateAsync(seoData);
            if (obj != null)
                return true;
            return false;


        }
    }
}
