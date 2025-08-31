using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.Dr.QuestionFolder;
using Core.Extention;
using Core.Service.Interface.Dr;
using Core.Service.Interface.Users;
using Core.Service.Services.Dr;
using Domain;
using Domain.Dr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStore.Base;

namespace DrMoradi.Areas.UserPanel.Controllers
{
    [Area(AreaName.UserPanel)]
    [Authorize]
    public class UserDietController : BaseController
    {
        private readonly IQuestion _question;
        private readonly IUser _User;
        private readonly IUserDiet _userDiet;
        private readonly IUserAnswer _userAnswer;
        private readonly IDiet _diet;
        private readonly IQuestionOption _questionOption;
        private readonly ISendDiet _sendDiet;
        private readonly IFileList _fileList;
        private readonly ILogger<UserDietController> _logger;
        public UserDietController(IQuestion question, IUserDiet userDiet, IUserAnswer userAnswer, IDiet diet, IQuestionOption questionOption, ISendDiet sendDiet, IFileList fileList, ILogger<UserDietController> logger, IUser user)
        {
            _question = question;
            _userDiet = userDiet;
            _userAnswer = userAnswer;
            _diet = diet;
            _questionOption = questionOption;
            _sendDiet = sendDiet;
            _fileList = fileList;
            _logger = logger;
            _User = user;
        }

        public async Task<IActionResult> Index(int DietId, int ParentId = 0)
        {
            _logger.LogInformation(EventIdList.Read,
           "نمایش فرم سوالات رژیم. DietId={DietId}, ParentId={ParentId}, UserId={UserId}",DietId, ParentId, User.GetUserId());

            if (DietId <= 0)
            {
                _logger.LogWarning("شناسه رژیم نامعتبر: {DietId}", DietId);

                return BadRequest("شناسه رژیم نامعتبر است.");
            }
        
            var diet = await _diet.GetDietById(DietId);
            if (diet == null)
            {
                _logger.LogWarning("رژیمی با شناسه {DietId} یافت نشد", DietId);
                return NotFound();
            }
            bool firstform = true;
            if (ParentId != 0)
            {
                firstform = false;
            }

           




            var HasDiet = await _userDiet.UserHasDiet(User.GetUserId(), DietId);
            var obj = await _question.GetQuestionByDietIdAsync(DietId, firstform);


            var res = new List<DynamicQuestionVm>();
            foreach (var q in obj.OrderBy(a => a.Order))
            {
                var options = await _questionOption.GetQuestionOptionsByQuestionId(q.Id);
                res.Add(new DynamicQuestionVm
                {
                    QuestionId = q.Id,
                    QuestionText = q.Name,
                    FieldType = q.FieldType,
                    Order = q.Order,
                    IsRequired = q.IsRequired,
                    DietId = DietId,
                    Answer = "",
                    Options = options.Select(o => new SelectListItem
                    {
                        Value = o.OptionText,
                        Text = o.OptionText
                    }).ToList()
                });
            }
            var data = new ShowQuestionToUserVM()
            {
                DietId = DietId,
                DietName = diet.Name,
                parentId = ParentId,
                Questions = res
            };
            if (HasDiet && ParentId == 0)
            {
                _logger.LogInformation("کاربر {UserId} قبلاً این رژیم را داشته است.", User.GetUserId());
                TempData["Double"] =
                    "شما قبلا این رژیم را تهیه نموده‌اید. در صورت درخواست تکرار، از پنل خود قسمت " +
                    "<a href='" + Url.Action("mydiet", "userDiet") + "'>رژیم‌های من</a> اقدام فرمائید در غیر اینصورت نام رژیم گیرنده را وارد نمایید .";
            }

            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertAnswer(ShowQuestionToUserVM vm, string? Username =null)
        {
            _logger.LogInformation("ثبت پاسخ کاربر {UserId} برای رژیم {DietId}", User.GetUserId(), vm.DietId);
            try
            {
                List<IFormFile> files = new List<IFormFile>();
                foreach (var q in vm.Questions)
                {
                    if (q.Attachments != null)
                    {
                        foreach (var file in q.Attachments)
                        {
                            files.Add(file);
                        }
                    }
                }
                var size = FileTools.GetAllSize(files);
                if (size / 1024 / 1024 > 2)
                {
                    _logger.LogWarning("حجم فایل های آپلود شده بیش از ۲ مگابایت است. UserId={UserId}", User.GetUserId());
                    TempData["UploadError"] = "حجم فایل های آپلودی نمی‌تواند از 2M بیشتر باشد";
                    return View("Index", await make(vm, vm.parentId.Value));
                }

                var formatcheck = FileTools.IsValidUploadedFile(files);
                if (!formatcheck)
                {
                    _logger.LogWarning("فایل های بارگذاری شده معتبر نمی باشند. UserId={UserId}", User.GetUserId());
                    TempData["UploadError"] = "فایل های بارگذاری شده معتبر نمی باشند";
                    return View("Index", await make(vm, vm.parentId.Value));

                }
                //model test
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("مدل ورودی معتبر نیست. UserId={UserId}", User.GetUserId());
                    return View("Index", await make(vm, vm.parentId.Value));
                }
                if (vm.parentId == 0)
                {
                    vm.Questions.Insert(0, new DynamicQuestionVm()
                    {
                        DietId = vm.DietId,
                        Answer = Username,
                        QuestionId = 1,
                        IsRequired = true,

                    });
                }

                var result = await _userDiet.InsertAnswerAsync(vm, User.GetUserId());

                if (!result)
                {
                    _logger.LogError("ثبت پاسخ رژیم برای کاربر {UserId} ناموفق بود. DietId={DietId}", User.GetUserId(), vm.DietId);
                    TempData[Error] = ErrorMessage;
                    return View("Index", await make(vm, vm.parentId.Value));
                }
                _logger.LogInformation("ثبت پاسخ رژیم برای کاربر {UserId} موفقیت‌آمیز بود. DietId={DietId}", User.GetUserId(), vm.DietId);
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index", "UserPanel");
            }
            catch (Exception ex)
            {
                _logger.LogError(EventIdList.Error, ex, "خطا در ثبت پاسخ کاربر {UserId} برای رژیم {DietId}", User.GetUserId(), vm.DietId);
                TempData[Error] = ErrorMessage;
                return View("Index", await make(vm, vm.parentId.Value));
            }
       
        }

        public async Task<IActionResult> MyDiet()
        {

            var obj = await _userDiet.GetAllDietsByUserId(User.GetUserId());
            return View(obj);
        }
        public async Task<IActionResult> GetDiet(int UserDietId)
        {
            ShowUserDietVm obj = new ShowUserDietVm()
            {
                SendDiet = await _sendDiet.GetSendDietByUserDietIdAndUserId(UserDietId, User.GetUserId()),
                fileLists = await _fileList.GetALlfileByUserDietId(UserDietId, false)
            };

            return View(obj);
        }
        private async Task<ShowQuestionToUserVM> make(ShowQuestionToUserVM vm,int ParentId)
        {
            bool firstform = true;
            if (ParentId != 0)
            {
                firstform = false;
            }
            var form = await _userDiet.UserHasDiet(User.GetUserId(), vm.DietId);
            var obj = await _question.GetQuestionByDietIdAsync(vm.DietId, firstform);


            var res = new List<DynamicQuestionVm>();
            foreach (var q in obj.OrderBy(a => a.Order))
            {
                var options = await _questionOption.GetQuestionOptionsByQuestionId(q.Id);
                res.Add(new DynamicQuestionVm
                {
                    QuestionId = q.Id,
                    QuestionText = q.Name,
                    FieldType = q.FieldType,
                    Order = q.Order,
                    IsRequired = q.IsRequired,
                    DietId = vm.DietId,
                    Answer = "",
                    Options = options.Select(o => new SelectListItem
                    {
                        Value = o.OptionText,
                        Text = o.OptionText
                    }).ToList()
                });
            }
            var data = new ShowQuestionToUserVM()
            {
                DietId = vm.DietId,
                DietName = vm.DietName,
                Questions = res
            };
            return data;
        }

    }

}
