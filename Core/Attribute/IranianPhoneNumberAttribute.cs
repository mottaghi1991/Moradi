using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Tools.Attribute
{
    public class IranianPhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string phoneNumber = value.ToString();
            var regex = new Regex(@"^09\d{9}$"); // فرمت صحیح ایران

            if (!regex.IsMatch(phoneNumber))
            {
                return new ValidationResult("شماره موبایل باید با 09 شروع شود و 11 رقم داشته باشد.");
            }

            return ValidationResult.Success;
        }
    }
}
