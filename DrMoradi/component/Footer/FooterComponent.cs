using Core.Dto.ViewModel.SettingDto;
using Core.Interface.Admin;
using Core.Interface.MainPage;
using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Footer
{
    public class FooterComponent:ViewComponent
    {
        private readonly ISetting _Setting;
        private readonly IDiet _diet;

        public FooterComponent(ISetting setting, IDiet diet)
        {
            _Setting = setting;
            _diet = diet;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = new ShowFooterVM
            {
                setting = await _Setting.GetSettingAsync(),   
                Diet = await _diet.GetAllByActiveAsync(true)
            };

            return View("~/Component/Footer/_SiteFooter.cshtml", vm);
        }
    }
}
