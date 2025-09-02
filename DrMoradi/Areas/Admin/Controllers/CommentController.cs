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
            var Comments = await _comment.GetAllCommentPaging(page, pagesize,User.GetUserId());

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
            var obj=await _comment.GetCommentbyid(commentId);
            if (obj == null)
                return NotFound("نظر یافت نشد.");
            var replay =await _comment.GetRepolaybyid(commentId);
      
            return View(new ShowCommentVm()
            {
                Id= commentId,
                IsApproved=obj.IsApproved,
                CreateDate = obj.CreateDate.ToPersian(),
                EntityType=obj.EntityType,
                Mobile= obj.myUser?.UserName ?? obj.Mobile,
                UserComment=obj.Text,
                AdminComment=replay!=null?replay.Text:"",
                DietId = obj.EntityId ?? 0
            });
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
            


        
    }
}
