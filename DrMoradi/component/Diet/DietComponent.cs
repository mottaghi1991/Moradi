using Core.Service.Interface.Dr;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Diet
{
    public class DietComponent:ViewComponent
    {
        private readonly IDiet _diet;

        public DietComponent(IDiet diet)
        {
            _diet = diet;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _diet.GetAllByActiveAsync(true);
            return View("~/Component/Diet/_Diet.cshtml", result);
        }
    }
}
