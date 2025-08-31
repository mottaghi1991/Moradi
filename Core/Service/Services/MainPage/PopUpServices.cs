using Core.Service.Interface.MainPage;
using Data.MasterInterface;
using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Services.MainPage
{
    public class PopUpServices : IPopUp
    {
        private readonly IMaster<PopUp> _master;

        public PopUpServices(IMaster<PopUp> master)
        {
            _master = master;
        }

        public async Task<bool> DeleteAsync(PopUp popUp)
        {
            return await _master.DeleteAsync(popUp);
        }

        public async Task<IEnumerable<PopUp>> GetAllPopUps()
        {
            return await _master.GetAllEfAsync();
        }

        public async Task<IEnumerable<PopUp>> GetAllPopUpWithStatus(bool IsActive)
        {
            return await _master.GetAllEfAsync(a => a.IsActive == IsActive);
        }

        public async Task<PopUp> GetPopById(int popid)
        {
            return _master.GetAllAsQueryable(a => a.Id == popid).FirstOrDefault();
        }

        public async Task<bool> InsertAsync(PopUp popUp)
        {
            return await _master.InsertAsync(popUp) != null;
        }

        public async Task<bool> UpdateAsync(PopUp popUp)
        {
            return await _master.UpdateAsync(popUp) != null;
        }
    }
}
