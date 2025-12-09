
using Domain.Main;
using System.Threading.Tasks;

namespace Core.Interface.MainPage
{
    public interface ISetting
    {

        public Task<Setting> GetSettingAsync();

        public Task<bool> UpdateSettingAsync(Setting setting);
    }
}