using Core.Dto.ViewModel.Dr;
using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.main;
using Core.Service.Interface.Dr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace DrMoradi.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private readonly IPost _post;

        public BlogController(IPost post)
        {
            _post = post;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            int pagesize = 9;
            var total = await _post.PostCount();
            var posts = await _post.GetAllPostPaging(page, pagesize);
        
            return View(new BlogPageVm()
            {
                Post = posts,
                Page = page,
                TotalPage = (int)Math.Ceiling((double)total / pagesize)
            });
        }
        [Route("blogDetails/{id:int}")]
        public async Task<IActionResult> blogDetails(int id)
        {
          
            return View(new ShowPostDetailvm()
            {
                post = await _post.GetpostById(id),
                TopPost = await _post.GetTopPost(4)
            });
        }     
    }
}
