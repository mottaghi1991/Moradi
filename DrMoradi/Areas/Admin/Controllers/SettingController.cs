using System.Drawing;
using AutoMapper;
using Core.Dto.ViewModel.Admin;
using Core.Extention;
using Core.Service.Interface.Admin;
using Core.Static;
using Data;
using Domain.PersonalData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebStore.Base;

namespace PersonalSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : BaseController
    {
        private readonly ISetting _setting;
        private readonly IMapper _mapper;
        private readonly IOptions<Setting> _Headersetting;
        public SettingController(ISetting setting, IMapper mapper, IOptions<Setting> headersetting)
        {
            _setting = setting;
            _mapper = mapper;
            _Headersetting = headersetting;
        }
        public async Task<IActionResult> Index()
        {
            var obj =await _setting.GetSettingAsync();

            return View(_mapper.Map<EditSettingViewModel>(obj));



        }
        [HttpPost]
        public async Task<IActionResult> Index(EditSettingViewModel setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }

         
            //setting.Birthday = "1991-08-20";
            var old =await _setting.GetSettingAsync();
            if (setting.Profile != null)
            {
                var filename = FileTools.GetFileName(setting.Profile);

                var path = FileTools.UploadFile(setting.Profile, filename, PathTools.Profile);
                if (!path.Success)
                {
                    TempData[Error] = "بارگذازی فایل با مشکل مواجه گردید";
                    return View(setting);
                    
                }
                FileTools.DeleteFile(old.ProfileImage);
                old.ProfileImage = path.FilePath;
            }
            old.jobs = setting.jobs;
            old.Birthday = setting.Birthday.ToMiladi();
            old.Location=setting.Location;
            old.Tweeter=setting.Tweeter;
            old.Aboute=setting.Aboute;
            old.Instagram=setting.Instagram;
            old.Linkedin=setting.Linkedin;
            old.Aboute= setting.Aboute;
            old.Email=setting.Email;
            old.Name=setting.Name;
            old.Phone=setting.Phone;
  
            
           
            if (await _setting.UpdateSettingAsync(old))
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData[Error] = ErrorMessage;
                return View(setting);
            }
        
        }
        [HttpPost]
        public IActionResult UploadImage(IFormFile upload)
        {
            if (upload.Length == 0)
                return Json(new { uploaded = false, error = new { message = "No file was uploaded." } });
            var filename = FileTools.GetFileName(upload);
            var path = FileTools.UploadFile(upload, filename, "uploader");
            return Json(new
            {
                uploaded = true,
                url = path
            });
        }
        [HttpPost]
        public IActionResult SaveImageInfo(string imageSrc, string alt)
        {
            if (ModelState.IsValid)
            {
                // ذخیره اطلاعات در پایگاه داده
              

                return Json(new { success = true });
            }
            return Json(new { success = true });
        }
       
    }
}
