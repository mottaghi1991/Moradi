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
        public static string ToPersianLong(this DateTime date)
        {

            PersianCalendar persianCalendar = new PersianCalendar();

            // استخراج سال، ماه و روز شمسی
            string persianYear = persianCalendar.GetYear(date).ToString();
            string persianMonth = persianCalendar.GetMonth(date).ToString();
            string persianDay = persianCalendar.GetDayOfMonth(date).ToString();
            string hour = persianCalendar.GetHour(date).ToString();
            string minute = persianCalendar.GetMinute(date).ToString();
            return $"{persianYear:0000}/{persianMonth:00}/{persianDay:00} {hour:00}:{minute:00}";
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
        public static string ToPersianDayOfWeek(this DateTime date)
        {
            // PersianCalendar خودش روز هفته رو مثل DateTime.DayOfWeek بر اساس میلادی برمی‌گردونه
            // پس برای نگاشت به فارسی فقط کافیه از enum DayOfWeek استفاده کنیم
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یک‌شنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه‌شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنج‌شنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                default:
                    return "";
            }
        }
    }
}
