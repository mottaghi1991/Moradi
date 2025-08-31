using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Slider
{
    public class PopUpComponent : ViewComponent
    {
        private readonly IPopUp _popUp;

        public PopUpComponent(IPopUp popUp)
        {
            _popUp = popUp;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _popUp.GetAllPopUpWithStatus(true);
            var popUp = result.FirstOrDefault();
            if (popUp == null)
            {
                // هیچ اعلانی نداریم
                return Content(string.Empty);
            }
            return View("~/Component/PopUp/_PopUp.cshtml", result.FirstOrDefault());
        }
    }
}
