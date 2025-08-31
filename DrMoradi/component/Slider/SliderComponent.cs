using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Slider
{
    public class SliderComponent:ViewComponent
    {
        private readonly ISlider _slider;

        public SliderComponent(ISlider slider)
        {
            _slider = slider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _slider.GetSliders();
            return View("~/Component/Slider/_Slider.cshtml", result);
        }
    }
}
