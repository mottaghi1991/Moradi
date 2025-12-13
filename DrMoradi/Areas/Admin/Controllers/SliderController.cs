using AutoMapper;
using Core.Dto.ViewModel.main;
using Core.Extention;
using Core.Service.Interface.MainPage;
using Domain.Dr;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    public class SliderController : BaseController
    {
        private readonly ISlider _slider;
        private readonly IMapper _mapper;

        public SliderController(ISlider slider, IMapper mapper)
        {
            _slider = slider;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _slider.GetSliders());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int SliderId)
        {
            var obj = await _slider.GetSliderById(SliderId);
            if (obj == null) return NotFound();

            return View(_mapper.Map<SliderEditVm>(obj));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SliderEditVm slider)
        {
            var old = await _slider.GetSliderById(slider.Id);
            if (slider.DesktopFileImage != null)
            {
                string filename = FileTools.GetFileName(slider.DesktopFileImage);
                var uploadfile = FileTools.UploadFile(slider.DesktopFileImage, filename, "Slider");
                if (uploadfile.Success)
                    old.DesktopFile = uploadfile.FilePath;
            }

            if (slider.MobileFileImage != null)
            {
                string filename = FileTools.GetFileName(slider.MobileFileImage);
                var uploadfile = FileTools.UploadFile(slider.MobileFileImage, filename, "Slider");
                if (uploadfile.Success)
                    old.MobileFile = uploadfile.FilePath;
            }
            old.DesktopDescript = slider.DesktopDescript;
            old.MobileDescript = slider.MobileDescript;
            old.Link = slider.Link;

            var result = await _slider.Update(old);
            if(result!=null)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            TempData[Error] = ErrorMessage;
            return View(slider);
        }
    }
}
