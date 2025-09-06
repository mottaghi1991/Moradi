using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.Dr.DietVM;
using Core.Dto.ViewModel.main;
using Core.Extention;
using Core.Service.Interface.Dr;
using Domain;
using Domain.Dr;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(areaName: AreaName.Admin)]
    public class DietOrderController : BaseController
    {
        private readonly IUserDiet _userDiet;
        private readonly IQuestion _question;
        private readonly IUserAnswer _userAnswer;
        private readonly ISendDiet _sendDiet;
        private readonly IFileList _fileList;
        private readonly ILogger<DietOrderController> _logger;
        public DietOrderController(IUserDiet userDiet, IQuestion question, IUserAnswer userAnswer, ISendDiet sendDiet, IFileList fileList, ILogger<DietOrderController> logger)
        {
            _userDiet = userDiet;
            _question = question;
            _userAnswer = userAnswer;
            _sendDiet = sendDiet;
            _fileList = fileList;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? userId, string fullName, string mobile, string paymentStatus = "Pay",int pageNumber = 1,int pageSize = 10)
        {
            
            
            
            _logger.LogInformation(EventIdList.Read, "Admin درخواست لیست همه سفارش‌ها");
            if (string.Equals(paymentStatus, "all", StringComparison.OrdinalIgnoreCase))
            {
                paymentStatus = null;
            }

            var result = await _userDiet.GetAllDietsByFilter(
            userId,           // همون ورودی کاربر
            paymentStatus,    // بعد از تبدیل "all" به null
            fullName,         // همون ورودی
            mobile,           // همون ورودی
            pageNumber,
            pageSize
        );

            return View(result);
        }



        public async Task<IActionResult> UserForm(int UserDietId)
        {
            _logger.LogInformation(EventIdList.Read, "نمایش فرم کاربر برای UserDietId={UserDietId}", UserDietId);
            var order = await _userDiet.GetUserDietById(UserDietId);
            if (order == null)
            {
                _logger.LogWarning(EventIdList.NotFound, "هیچ رژیمی با UserDietId={UserDietId} پیدا نشد", UserDietId);
                return NotFound();

            }
            var UserDietList = await _userDiet.GetAllParentAndChild(UserDietId);
            //if (!UserDietList.Any())
            //    return NotFound("رژیم ثبت شده‌ای برای این کاربر یافت نشد.");
            //if (UserDietId == 0)
            //{
            //    UserDietId = UserDietList.First().Id;
            //}
            //var formname =await _userAnswer.getNameFiel(order.UserId, UserDietId);
            ShowUserFormVM obj = new ShowUserFormVM()
            {
                UserId = order.UserId,
                userDiets = UserDietList,
                UserFile = await _fileList.GetALlfileByUserDietId(UserDietId, true),
                showUserAnswerVMs = await _userAnswer.GetUserAnswerByUserIdAndUserDietId(order.UserId, UserDietId)
            };

            _logger.LogInformation(EventIdList.Read, "فرم کاربر {UserId} برای رژیم {UserDietId} آماده نمایش است", order.UserId, UserDietId);
            return View(obj);
        }
        [HttpGet]
        public async Task<IActionResult> SendDiet(int UserDietId)
        {
            _logger.LogInformation(EventIdList.Read, "درخواست ارسال رژیم برای UserDietId={UserDietId}", UserDietId);
            if (UserDietId <= 0)
            {
                _logger.LogWarning(EventIdList.NotFound, "شناسه رژیم نامعتبر است: {UserDietId}", UserDietId);
                return BadRequest("شناسه رژیم نامعتبر است.");

            }
            var send = await _sendDiet.GetSendDietByUserDietId(UserDietId);
            ViewBag.File = await _fileList.GetALlfileByUserDietId(UserDietId, false);
            return View(new SendDiet()
            {
                UserDietId = UserDietId,
                Descript = send != null ? send.Descript : ""
            });
        }
        [HttpPost]
        public async Task<IActionResult> SendDiet(SendDiet sendDiet)
        {
            _logger.LogInformation(EventIdList.InsertId, "ثبت یا ویرایش رژیم ارسالی برای UserDietId={UserDietId}", sendDiet.UserDietId);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning(EventIdList.Error, "مدل ارسالی برای رژیم نامعتبر است. UserDietId={UserDietId}", sendDiet.UserDietId);
                return View(sendDiet);
            }
            Boolean result = false;
            try
            {
                var old = await _sendDiet.GetSendDietByUserDietId(sendDiet.UserDietId);
                if (old == null)
                {
                    result = await _sendDiet.InsertSendDiet(sendDiet);
                    _logger.LogInformation(EventIdList.InsertId, "رژیم جدید ارسال شد. UserDietId={UserDietId}", sendDiet.UserDietId);
                }
                else
                {
                    old.Descript = sendDiet.Descript;
                    result = await _sendDiet.UpdateSendDiet(old);
                    _logger.LogInformation(EventIdList.UpdateId, "رژیم موجود بروزرسانی شد. UserDietId={UserDietId}", sendDiet.UserDietId);
                }

                if (result)
                {
                    await _userDiet.UpdateToSend(sendDiet.UserDietId);
                    TempData[Success] = SuccessMessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError(EventIdList.Error, "ثبت یا ویرایش رژیم ارسالی با شکست مواجه شد. UserDietId={UserDietId}", sendDiet.UserDietId);
                    TempData[Error] = ErrorMessage;
                    return View(sendDiet);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(EventIdList.Error, ex, "خطا در پردازش ارسال رژیم. UserDietId={UserDietId}", sendDiet.UserDietId);
                TempData[Error] = "خطایی در ثبت اطلاعات رخ داد.";
                return View(sendDiet);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int UserDietId)
        {
            _logger.LogInformation(EventIdList.InsertId, "بارگذاری فایل برای UserDietId={UserDietId} و فایل با آدرس file={file}", UserDietId,file.Name);
            if (UserDietId <= 0)
            {
                _logger.LogWarning(EventIdList.Error, "شناسه رژیم نامعتبر برای آپلود فایل: {UserDietId}", UserDietId);
                return BadRequest("شناسه رژیم نامعتبر است.");

            }
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning(EventIdList.Error, "هیچ فایلی برای UserDietId={UserDietId} آپلود نشد", UserDietId);
                return BadRequest("هیچ فایلی ارسال نشده است.");

            }



            var fileName = FileTools.GetFileName(file);

            var FileResult = FileTools.UploadFile(file, fileName, "Attachment");
            if (!FileResult.Success)
            {
                _logger.LogError(EventIdList.Error, "بارگذاری فایل {FileName} با مشکل مواجه شد. UserDietId={UserDietId}", fileName, UserDietId);
                ModelState.AddModelError("ImageFile", "بارگذازی فایل با مشکل مواجه گردید");
                return Json(new { fileName });
            }
            var filePath = FileResult.FilePath;
            await _fileList.InsertFile(new FileList()
            {
                File = filePath,
                UserDietId = UserDietId,
                UserFile = false,

            });
            _logger.LogInformation(EventIdList.InsertId, "فایل {FileName} با موفقیت بارگذاری و ذخیره شد. UserDietId={UserDietId}", fileName, UserDietId);
            // نام فایل را برمی‌گردانیم
            return Json(new { fileName });


        }


        [HttpPost]
        public async Task<IActionResult> Delete(string fileName)
        {
            _logger.LogInformation(EventIdList.DeleteId, "حذف فایل {FileName}", fileName);
            try
            {
                if (await _fileList.deleteFile(fileName))
                {
                    if (FileTools.DeleteFile("FileUpload/Attachment/" + fileName))
                    {
                        _logger.LogInformation(EventIdList.DeleteId, "فایل {FileName} با موفقیت حذف شد", fileName);
                        return Json(new { success = true });

                    }
                    else
                    {
                        _logger.LogWarning(EventIdList.Error, "حذف فیزیکی فایل {FileName} ناموفق بود", fileName);
                        return Json(new { success = false });

                    }

                }
                else
                {
                    _logger.LogInformation(EventIdList.Info, "حذف رکورد فایل {FileName} از دیتابیس ناموفق بود", fileName);
                    return Json(new { success = false });
                }
            }
            catch
            {
                _logger.LogWarning(EventIdList.Error, "حذف رکورد فایل {FileName} از دیتابیس ناموفق بود", fileName);
                return Json(new { success = false });
            }



        }

    }
}
