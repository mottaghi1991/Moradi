using Domain;
using Domain.Dr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.ViewModel.Dr.QuestionFolder
{
    public class DynamicQuestionVm : IValidatableObject
    {

        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public FieldType FieldType { get; set; }
        public string Answer { get; set; }
        public decimal Order { get; set; }
        public bool IsRequired { get; set; }
        public int DietId { get; set; }
        public List<SelectListItem> Options { get; set; }

        public List<IFormFile> Attachments { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsRequired && string.IsNullOrWhiteSpace(Answer))
            {
                // ValidationResult می‌پرازد و نام property هم Answer است
                yield return new ValidationResult(
                    "پر کردن این گزینه اجباری می‌باشد",
                    new[] { nameof(Answer) }
                );
            }
        }
    }
}
