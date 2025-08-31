using Domain.PersonalData;
using System.Threading.Tasks;

namespace Core.Service.Interface.Admin
{
    public interface ISetting
    {
        public Task<Setting> GetSettingAsync();
   
        public Task<bool> UpdateSettingAsync(Setting setting);
    }
}