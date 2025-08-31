using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extention
{
    public static class DateTools
    {
        public static string ToPersian(this DateTime date)
        {

            PersianCalendar persianCalendar = new PersianCalendar();

            // استخراج سال، ماه و روز شمسی
            string persianYear = persianCalendar.GetYear(date).ToString();
            string persianMonth = persianCalendar.GetMonth(date).ToString();
            string persianDay = persianCalendar.GetDayOfMonth(date).ToString();
            return persianYear + "/" + persianMonth + "/" + persianDay;
        }

        public static DateTime ToMiladi(this string PersianDate)
        {
            var seprate = PersianDate.Split("/");
            var year =Convert.ToInt32(seprate[0]);
            var month =Convert.ToInt32(seprate[1]);
            var day =Convert.ToInt32(seprate[2]);
            PersianCalendar persianCalendar = new PersianCalendar();

            // تبدیل به تاریخ میلادی
            DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            return gregorianDate;
        }
    }
}
