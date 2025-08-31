using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Core.Service.Interface.Shop;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Slider
{
    public class CategoryComponent : ViewComponent
    {
        private readonly ICategory _category;

        public CategoryComponent(ICategory category)
        {
            _category = category;
        }

     

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _category.GetAllByActive(true);
            return View("~/Component/Category/_Category.cshtml", result);
        }
    }
}
