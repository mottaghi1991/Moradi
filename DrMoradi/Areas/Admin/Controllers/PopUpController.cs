using AutoMapper;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.main;
using Core.Extention;
using Core.Service.Interface.MainPage;
using Domain.Dr;
using Domain.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(areaName:AreaName.Admin)]
    [Authorize]
    public class PopUpController : BaseController
    {
        private readonly IPopUp _popUp;
        private readonly IMapper _mapper;

        public PopUpController(IPopUp popUp, IMapper mapper)
        {
            _popUp = popUp;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _popUp.GetAllPopUps());
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PopUpVm upVm)
        {
            if (!ModelState.IsValid)
            {
                return View(upVm);
            }
            string FIleName, FilePAth = null;
            if (upVm.ImageFile!=null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(upVm.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                    return View(upVm);
                }
                try
                {
                    FileTools.ChechSize(upVm.ImageFile, 1);

                }
                catch
                {
                    ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                    return View(upVm);
                }
               
                FIleName = FileTools.GetFileName(upVm.ImageFile);

                var FileResult = FileTools.UploadFile(upVm.ImageFile, FIleName, "popUp");
                if (!FileResult.Success)
                {
                    ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                    return View(upVm);
                }
                FilePAth = FileResult.FilePath;
                upVm.Image = FilePAth;
            }
            var Result = await _popUp.InsertAsync(_mapper.Map<PopUp>(upVm));
            if (Result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            if (upVm.ImageFile != null)
            {
                FileTools.DeleteFile(FilePAth);
            }
            TempData[Error] = ErrorMessage;
            return View(upVm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int PopId)
        {
            if(PopId <= 0)
                return NotFound();
            var pop=await _popUp.GetPopById(PopId);
            if (pop == null)
                return NotFound();

            return View(_mapper.Map<PopUpEditVm>(pop));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PopUpEditVm upVm)
        {
            if (!ModelState.IsValid)
            {
                return View(upVm);
            }
            var old =await _popUp.GetPopById(upVm.Id);
            var oldFilePath = old.Image;

            string newFilePath = null;
            string FIleName, FilePAth = null;
            if (upVm.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(upVm.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                    return View(upVm);
                }
                try
                {
                    FileTools.ChechSize(upVm.ImageFile, 1);

                }
                catch
                {
                    ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                    return View(upVm);
                }

                FIleName = FileTools.GetFileName(upVm.ImageFile);

                var FileResult = FileTools.UploadFile(upVm.ImageFile, FIleName, "popUp");
                if (!FileResult.Success)
                {
                    ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                    return View(upVm);
                }
                newFilePath = FileResult.FilePath;
                old.Image = newFilePath;
            }
            old.Title = upVm.Title;
            old.Descript = upVm.Descript;
            old.IsActive = upVm.IsActive;
            var Result = await _popUp.UpdateAsync(_mapper.Map<PopUp>(old));
            if (Result)
            {
                if (upVm.ImageFile != null && !string.IsNullOrWhiteSpace(oldFilePath))
                    FileTools.DeleteFile(oldFilePath); TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            if (!string.IsNullOrWhiteSpace(newFilePath))
                FileTools.DeleteFile(newFilePath);
            TempData[Error] = ErrorMessage;
            return View(upVm);
        }
        public async Task<IActionResult> Delete(int PopId)
        {
            var pop =await _popUp.GetPopById(PopId);
            if (pop == null)
                return NotFound();
            var result =await _popUp.DeleteAsync(pop);
            if (result)
            {
                FileTools.DeleteFile(pop.Image);
                TempData[Success] =SuccessMessage;
                return RedirectToAction("Index");
            }
          
            TempData[Error] = ErrorMessage;
            return RedirectToAction("Index");
        }
    }
}
