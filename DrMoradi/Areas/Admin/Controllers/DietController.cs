using AutoMapper;
using Core.Dto.ViewModel.Admin.Role;
using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.Dr.QuestionFolder;
using Core.Extention;
using Core.Service.Interface.Dr;
using Domain.Dr;
using Domain.User;
using Domain.User.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(areaName: AreaName.Admin)]
    public class DietController : BaseController
    {
        private readonly IDiet _diet;
        private readonly IMapper _Mapper;
        private readonly IQuestion _question;
        private readonly IUserDiet _userDiet;
        private readonly IQuestionOption _questionOption;

        public DietController(IDiet diet, IMapper mapper, IQuestion question, IUserDiet userDiet, IQuestionOption questionOption)
        {
            _diet = diet;
            _Mapper = mapper;
            _question = question;
            _userDiet = userDiet;
            _questionOption = questionOption;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var diets = await _diet.GetAllAsync();
                return View(diets);
            }
            catch (Exception ex)
            {
                TempData[Error] = "در بارگذاری لیست رژیم‌ها خطایی رخ داد. لطفا بعداً تلاش کنید.";
                // اینجا Optionally لاگ کن ex
                return View(new List<Diet>());
            }
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddDietVm addDietVm)
        {
            if (!ModelState.IsValid)
            {
                return View(addDietVm);
            }
            if (addDietVm.ImageFile == null || addDietVm.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "انتخاب تصویر الزامی است");
                return View(addDietVm);
            }
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(addDietVm.ImageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                return View(addDietVm);
            }
            try
            {
                FileTools.ChechSize(addDietVm.ImageFile, 1);

            }
            catch
            {
                ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                return View(addDietVm);
            }
       
           

           
            string FIleName, FilePAth = null;
            FIleName = FileTools.GetFileName(addDietVm.ImageFile);
     
           var FileResult = FileTools.UploadFile(addDietVm.ImageFile, FIleName, "Diet");
            if (!FileResult.Success)
            {
                ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                return View(addDietVm);
            }
              FilePAth=FileResult.FilePath;
            addDietVm.Image = FilePAth;
            var Result = await _diet.InsertAsync(_Mapper.Map<Diet>(addDietVm));
            if (Result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            FileTools.DeleteFile(FilePAth);
            TempData[Error] = ErrorMessage;
            return View(addDietVm);
        }
        public async Task<IActionResult> Edit(int DietId)
        {
            var dite = await _diet.GetDietById(DietId);
            if (dite == null)
                return NotFound();
            return View(_Mapper.Map<EditDietVm>(dite));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDietVm editvm)
        {
            if (!ModelState.IsValid)
            {
                return View(editvm);
            }
            var Diet =await _diet.GetDietById(editvm.Id);
            var oldimage=Diet.Image;
            if (editvm.ImageFile != null )
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var ext = Path.GetExtension(editvm.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("ImageFile", "فرمت فایل تصویر مجاز نیست.");
                    return View(editvm);
                }
                try
                {
                    FileTools.ChechSize(editvm.ImageFile, 1);

                }
                catch
                {
                    ModelState.AddModelError("ImageFile", "حجم فایل نمیتواند بیشتر از 1M باشد");
                    return View(editvm);
                }

                string FIleName, FilePAth = null;
                FIleName = FileTools.GetFileName(editvm.ImageFile);
                var FileResult = FileTools.UploadFile(editvm.ImageFile, FIleName, "Diet");
                if (!FileResult.Success)
                {
                    ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                    return View(editvm);
                }
                Diet.Image = FileResult.FilePath;
            }
            Diet.Status=editvm.Status;
            Diet.Name=editvm.Name;
            Diet.Descript=editvm.Descript;
            Diet.Price=editvm.Price;
            Diet.SpecialDescript=editvm.SpecialDescript;
            Diet.Video = editvm.Video;
           




         
        
            var Result = await _diet.UpdateAsync(Diet);
            if (Result)
            {
                if (editvm.ImageFile != null)
                    FileTools.DeleteFile(oldimage);
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            if(oldimage!=Diet.Image)
                FileTools.DeleteFile(Diet.Image);
            TempData[Error] = ErrorMessage;
            return View(editvm);
        }
        [HttpGet]
        public async Task<IActionResult> DietQuestion(int DietId,bool FirstForm)
        {
            try
            {
                var Diet = await _diet.GetDietById(DietId);
                if (Diet == null)
                {
                    return NotFound();
                }
                ViewBag.Questions = await _question.GetAllAsync();
                var obj = new DietQuestionVm()
                {
                    DietId = DietId,
                    DietName = Diet.Name,
                    FirstForm = FirstForm,
                    DietQuestion = await _question.GetDietQuestionByDietIdAsync(DietId, FirstForm)
                };
                return View(obj);
            }
            catch (Exception ex) {
                TempData[Error] = "در بارگذاری اطلاعات خطایی رخ داد.";
                return RedirectToAction("Index");
            }
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DietQuestion(int DietId,List<int> QuestionList,bool FirstForm=false)
        {
            if (QuestionList == null||DietId<=0)
            {
                TempData[Error] = "هیچ سوالی انتخاب نشده است.";
                return RedirectToAction("DietQuestion", new { DietId });
            }
           var result=await _diet.UpdateDietQuestionsAsync(DietId, QuestionList, FirstForm);
            if(result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData[Error] = ErrorMessage;
                return RedirectToAction("DietQuestion", new { DietId = DietId });
            }

        }
        [HttpGet]
        public async Task<IActionResult> ShowDiet(int DietId,bool FirstForm)
        {
            if (DietId <= 0)
            {
                TempData[Error] = "شناسه رژیم نامعتبر است.";
                return RedirectToAction("Index");
            }

            var diet = await _diet.GetDietById(DietId);
            if (diet == null)
            {
                return NotFound();
            }

            var dietQuestions = await _question.GetQuestionByDietIdAsync(DietId, FirstForm);

            var result = new List<DynamicQuestionVm>();

            foreach (var question in dietQuestions.OrderBy(q => q.Order))
            {
                var options = await _questionOption.GetQuestionOptionsByQuestionId(question.Id);

                result.Add(new DynamicQuestionVm
                {
                    QuestionId = question.Id,
                    QuestionText = question.Name,
                    FieldType = question.FieldType,
                    Order = question.Order,
                    IsRequired = question.IsRequired,
                    DietId = DietId,
                    Answer = "",
                    Options = options.Select(opt => new SelectListItem
                    {
                        Value = opt.Id.ToString(),
                        Text = opt.OptionText
                    }).ToList()
                });
            }
            var a = new ShowQuestionToUserVM()
            {
                DietId = DietId,
                DietName = diet.Name,
                Questions = result
            };
            return View(a);

        }

    }
}
