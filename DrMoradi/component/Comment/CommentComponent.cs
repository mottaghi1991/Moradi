using Core.Dto.ViewModel.main;
using Core.Service.Interface.Dr;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DrMoradi.Views.Shared.component.Diet
{
    public class CommentComponent : ViewComponent
    {
       

       
        public async Task<IViewComponentResult> InvokeAsync(UsualComentVM model = null)
        {
            if (model == null)
                model = new UsualComentVM();
            return View("~/Component/Comment/_Comment.cshtml",model);
        }
    }
}
