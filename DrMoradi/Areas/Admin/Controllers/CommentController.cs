using AspNetCoreGeneratedDocument;
using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.main;
using Core.Extention;
using Core.Service.Interface.MainPage;
using Domain.Dr;
using Domain.Main;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area(AreaName.Admin)]
    public class CommentController : BaseController
    {
        private readonly IComment _comment;

        public CommentController(IComment comment)
        {
            _comment = comment;
        }

        public async Task<IActionResult> Index(int page = 1)
        {


            int pagesize = 10;
            var total = await _comment.PostCount();
            var Comments = await _comment.GetAllCommentPaging(page, pagesize, User.GetUserId());

            return View(new CommentPageVm()
            {
                Comments = Comments,
                Page = page,
                TotalPage = (int)Math.Ceiling((double)total / pagesize)
            });
            //var comments = await _comment.GEtAllUserComments() ?? new List<Comment>();
            //return View(comments);
        }
        [HttpGet]
        public async Task<IActionResult> ReplyComment(int commentId)
        {
            if (commentId <= 0)
                return BadRequest("شناسه نظر نامعتبر است.");
            var obj = await _comment.ReplyComment(commentId);
            if (obj == null)
                return NotFound("نظر یافت نشد.");

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyComment(ShowCommentVm comment)
        {
            if (!ModelState.IsValid)
                return View(comment);
            try
            {
                var result = await _comment.ReplyToCommentAsync(comment, User.GetUserId());
                if (result)
                {
                    TempData[Success] = SuccessMessage;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[Error] = Error;
                    return View(comment);
                }
            }
            catch (Exception ex)
            {
                // اینجا باید توی لاگ ذخیره بشه
                TempData[Error] = "خطای سیستمی در ثبت پاسخ.";
                return View(comment);
            }






        }
        public async Task<IActionResult> Delete(int CommentId)
        {
            var result = await _comment.Delete(CommentId);
            if (result)
            {
                TempData[Success] = SuccessMessage;
                return RedirectToAction("Index");
            }
            TempData[Error] = ErrorMessage;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListQuestion()
        {
            return View(await _comment.GetQuestion());
        }
        [HttpGet]
        public IActionResult InsertQuestion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> InsertQuestion(InsertQuestionVM Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var result = await _comment.InsertQuestion(Model, User.GetUserId());
            if (result.ErrorId != 0)
            {
                TempData[Error] = result.ErrorTitle;
                return View(Model);
            }
            TempData[Success] = result.ErrorTitle;
            return RedirectToAction("ListQuestion");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateQuestion(int CommentId)
        {
            var obj = await _comment.ReplyComment(CommentId);
            var model = await _comment.GetCommentbyid(CommentId);
            if (model == null) return NotFound();
            return View(new InsertQuestionVM()
            {
                CommentId = CommentId,
                Question = obj.UserComment,
                Answer = obj.AdminComment
            });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuestion(InsertQuestionVM Model)
        {
            if (!ModelState.IsValid) return View(Model);
            var result = await _comment.UpdateQuestion(Model, User.GetUserId());
            if (result.ErrorId != 0)
            {
                TempData[Error] = result.ErrorTitle;
                return View(Model);
            }
            TempData[Success] = result.ErrorTitle;
            return RedirectToAction("ListQuestion");
        }
        public async Task<IActionResult> DeleteQuestion(int CommentId)
        {
            var model = await _comment.GetCommentbyid(CommentId);
            if (model == null) return NotFound();
            var result = await _comment.DeleteQuestion(CommentId);
            if (result.ErrorId != 0)
            {
                TempData[Error] = result.ErrorTitle;
                return RedirectToAction("ListQuestion");
            }
            TempData[Success] = result.ErrorTitle;
            return RedirectToAction("ListQuestion");
        }
    }
}
