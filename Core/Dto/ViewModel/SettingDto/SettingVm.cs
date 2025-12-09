using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Dto.ViewModel.Admin.SettingDto
{
    public class SettingVm
    {
        public int Id { get; set; }
        [DisplayName("لوگو")]
        public string Logo { get; set; }
        [DisplayName("لوگو")]
        public IFormFile LogoFile { get; set; }
        [DisplayName("آدرس نقشه")]
        public string MapAddress { get; set; }
        [DisplayName("آدرس")]
        public string Address { get; set; }
        [DisplayName("ایمیل")]
        public string Email { get; set; }
        [DisplayName("موبایل")]
        public string Mobile { get; set; }

        [DisplayName("شماره تلفن 1")]
     
        public string Number1 { get; set; }
        [DisplayName("شماره تلفن 2")]
        public string Number2 { get; set; }
        [DisplayName("نام سایت")]
        public string SiteName { get; set; }
        [DisplayName("ساعت کاری")]
        public string WorkTime { get; set; }
        [DisplayName("توضیحات فوتر")]
        public string FooterDescript { get; set; }
        [DisplayName("تصویر بنر")]
        public string MainBanner { get; set; }
        [DisplayName("تصویر بنر")]
        public IFormFile MainBannerFile { get; set; }
        [DisplayName("آدرس بنر")]
        public string MainBannerAddress { get; set; }
    }
}
