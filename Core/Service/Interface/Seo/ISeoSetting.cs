using Domain.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.Seo
{
    public interface ISeoSetting
    {
        Task<SeoData> GetPublicData();
        Task<SeoData> GetSeoDataByURL(string Url);
        Task<bool> update(SeoData seoData);
        Task<bool> Insert(SeoData seoData);
        
    }
}
