using Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interface.MainPage
{
    public interface IPopUp
    {
        public Task<IEnumerable<PopUp>> GetAllPopUps();
        public Task<IEnumerable<PopUp>> GetAllPopUpWithStatus(bool IsActive);
        public Task<bool> InsertAsync(PopUp popUp); 
        public Task<bool> UpdateAsync(PopUp popUp);
        public Task<bool> DeleteAsync(PopUp popUp);
        public Task<PopUp> GetPopById(int popid);
    }
}
