using AutoMapper;
using Core.Dto.ViewModel.Admin;
using Core.Dto.ViewModel.Admin.SettingDto;
using Core.Extention;
using Core.Interface.Admin;
using Core.Interface.MainPage;
using Core.Service.Interface.Admin;
using Core.Service.Interface.Seo;
using Core.Static;
using Core.Tools;
using Domain.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using WebStore.Base;

namespace Mandella.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : BaseController
    {
        private readonly ISetting _setting;
        private readonly IMapper _mapper;
        private readonly IOptions<Setting> _Headersetting;
        private readonly ISeoSetting _seoSetting;
        public SettingController(ISetting setting, IMapper mapper, IOptions<Setting> headersetting, ISeoSetting seoSetting)
        {
            _setting = setting;
            _mapper = mapper;
            _Headersetting = headersetting;
            _seoSetting = seoSetting;
        }
        public async Task<IActionResult> Index()
        {
            var obj = await _setting.GetSettingAsync();

            return View(_mapper.Map<SettingVm>(obj));



        }
        [HttpPost]
        public async Task<IActionResult> Edit(SettingVm setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }
            var old = await _setting.GetSettingAsync();
            if(setting.LogoFile!=null)
            {
                var Logoname = FileTools.GetFileName(setting.LogoFile);
                var path = FileTools.UploadFile(setting.LogoFile, Logoname, PathTools.Logo);
                old.Logo = path.FilePath;
            }
            if (setting.MainBannerFile != null)
            {
                var Bannername = FileTools.GetFileName(setting.MainBannerFile);
                var path = FileTools.UploadFile(setting.LogoFile, Bannername, "Banner");
                old.MainBanner = path.FilePath;
            }
            if (setting.IconFirstFile != null)
            {
                var Bannername = FileTools.GetFileName(setting.IconFirstFile);
                var path = FileTools.UploadFile(setting.IconFirstFile, Bannername, "Icon");
                old.IconFirst = path.FilePath;
            }
            if (setting.IconSecondFile != null)
            {
                var Bannername = FileTools.GetFileName(setting.IconSecondFile);
                var path = FileTools.UploadFile(setting.IconSecondFile, Bannername, "Icon");
                old.IconSecond = path.FilePath;
            }
            if (setting.IconThirdFile != null)
            {
                var Bannername = FileTools.GetFileName(setting.IconThirdFile);
                var path = FileTools.UploadFile(setting.IconThirdFile, Bannername, "Icon");
                old.IconThird = path.FilePath;
            }
            old.IconFirstLink = setting.IconFirstLink;
            old.IconSecondLink = setting.IconSecondLink;
            old.IconThirdLink = setting.IconThirdLink;
            old.Number1 = setting.Number1;
            old.Number2 = setting.Number2;
            old.Mobile = setting.Mobile;
            old.Email = setting.Email;
            old.MainBannerAddress = setting.MainBannerAddress;
            old.MapAddress = setting.MapAddress;
            old.SiteName = setting.SiteName;
            old.Address = setting.Address;
            old.WorkTime = setting.WorkTime;
            old.FooterDescript = setting.FooterDescript;
            old.ShowBlog = setting.ShowBlog;
           var result= await _setting.UpdateSettingAsync(old);
            if (result)
                TempData[Success] = SuccessMessage;
            else
                TempData[Error] = ErrorMessage;
            return RedirectToAction("Index");
        }
      
       
        //[HttpGet]
        //public async Task<IActionResult> SeoSetting()
        //{
        //    var obj = await _seoSetting.GetPublicData();
        //    return View(_mapper.Map<SeoSettingAddVm>(obj));
        //}
        //[HttpPost]
        //public async Task<IActionResult> SeoSetting(SeoSettingAddVm seoSetting)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(seoSetting);
        //    }
        //    var obj = await _seoSetting.GetPublicData();

        //    if (seoSetting.OgImageFile != null)
        //    {
        //        string FIleName, FilePAth = null;
        //        FIleName = FileTools.GetFileName(seoSetting.OgImageFile);
        //        FilePAth = FileTools.UploadFile(seoSetting.OgImageFile, FIleName, "OgImage");
        //        FileTools.DeleteFile(obj.OgImage);
        //        obj.OgImage = seoSetting.CanonicalUrl + FilePAth;
        //    }
        //    obj.MetaTitle = seoSetting.MetaTitle;
        //    obj.MetaDescription = seoSetting.MetaDescription;
        //    obj.MetaKeywords = seoSetting.MetaKeywords;
        //    obj.CanonicalUrl = seoSetting.CanonicalUrl;
        //    obj.OgTitle = seoSetting.OgTitle;
        //    obj.OgDescription = seoSetting.OgDescription;
        //    var json = JsonLdBuilder.JsonLdSchemaBuilder.GenerateHomePageSchema(SiteURL, "سایت شخصی علی متقی", obj.MetaDescription);
        //    obj.JsonLdSchema = json;
        //    var result = await _seoSetting.update(obj);
        //    if (result)
        //    {
        //        TempData[Success] = SuccessMessage;
        //        return RedirectToAction("SeoSetting");
        //    }
        //    else
        //    {
        //        TempData[Error] = ErrorMessage;
        //        return View(seoSetting);
        //    }

        //}
    }
}

