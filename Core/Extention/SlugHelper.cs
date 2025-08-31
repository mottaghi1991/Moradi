using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Extention
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string phrase)
        {
            string str = phrase.ToLowerInvariant();

            str = RemoveDiacritics(str); // حذف علائم فارسی
            str = Regex.Replace(str, @"[^a-z0-9\u0600-\u06FF\s-]", ""); // حذف نمادها
            str = Regex.Replace(str, @"\s+", " ").Trim(); // حذف فاصله‌های اضافی
            str = str.Replace(" ", "-");

            return str;
        }
        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            return new string(chars.ToArray());
        }
    }
}
