using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Core.Service.Interface.Shop;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Slider
{
    public class PostComponent : ViewComponent
    {
        private readonly IPost _post;

        public PostComponent(IPost post)
        {
            _post = post;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _post.GetTopPost(5);
            return View("~/Component/Post/_Post.cshtml", result);
        }
    }
}
