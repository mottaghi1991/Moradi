using Core.Service.Interface.Dr;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Diet
{
    public class NavbarComponent:ViewComponent
    {
        private readonly IDiet _diet;

        public NavbarComponent(IDiet diet)
        {
            _diet = diet;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _diet.GetAllAsync();
            return View("~/Component/Navbar/_Navbar.cshtml", result);
        }
    }
}
