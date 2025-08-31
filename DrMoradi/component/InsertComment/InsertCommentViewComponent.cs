using Core.Dto.ViewModel.main;
using Core.Service.Interface.MainPage;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DrMoradi.component.InsertComment
{
    public class InsertCommentViewComponent:ViewComponent
    {
        private readonly IComment _comment;

        public InsertCommentViewComponent(IComment comment)
        {
            _comment = comment;
        }

        public async Task<IViewComponentResult> InvokeAsync(int entityId, EntityType entityType)
        {
            var comments =await _comment.GetCommentsByEntityId(entityId, entityType);
                

            var vm = new CommentsViewModel
            {
                EntityId = entityId,
                EntityType = entityType,
                Comments = comments
            };

            return View("~/Component/InsertComment/_InsertViewComponent.cshtml", vm);
        }
    }
}
