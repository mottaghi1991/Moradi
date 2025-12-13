using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class SliderEditVm
    {
        public int Id { get; set; }
        [DisplayName("تصویر مناسب دسکتاپ")]
        public string DesktopFile { get; set; }
        [DisplayName("تصویر مناسب دستکتاپ")]
        public IFormFile DesktopFileImage { get; set; }
        [DisplayName("توضیحات جایگزین دسکتاپ")]
        public string DesktopDescript { get; set; }
        [DisplayName("تصویر مناسب موبایل")]
        public string MobileFile { get; set; }
        [DisplayName("تصویر مناسب موبایل")]
        public IFormFile MobileFileImage { get; set; }
        [DisplayName("توضیحات جایگزین موبایل")]
        public string MobileDescript { get; set; }
        [DisplayName("لینک")]
        public string Link { get; set; }
    }
}
