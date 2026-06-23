using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Main
{
    public class Setting:Base
    {

       
        public string Logo { get; set; }
        public string SiteName { get; set; }
        public string MapAddress { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string WorkTime { get; set; }
        public string Number1 { get; set; }
        public string Number2 { get; set; }
        public string FooterDescript { get; set; }
  
        public string MainBanner { get; set; }
        public string MainBannerAddress { get; set; }
        public string IconFirst { get; set; }
        public string IconFirstLink { get; set; }
        public string IconSecond { get; set; }
        public string IconSecondLink { get; set; }
        public string IconThird { get; set; }
        public string IconThirdLink { get; set; }
        public bool ShowBlog { get; set; }

    }
}
