using Core.Interface.Store;
using Core.Service.Interface.Dr;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Diet
{
    public class ProductComponent : ViewComponent
    {
        private readonly IProduct _Product;

        public ProductComponent(IProduct product)
        {
            _Product= product;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _Product.GetProductBybcategory(1);
            return View("~/Component/Product/_Product.cshtml", result);
        }
    }
}
