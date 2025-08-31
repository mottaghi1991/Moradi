using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.main
{
    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; } // مسیر در صورت موفقیت
        public string ErrorMessage { get; set; } // پیام خطا در صورت شکست
    }
}
