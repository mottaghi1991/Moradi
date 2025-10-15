using Microsoft.AspNetCore.Mvc;
using WebStore.Base;

namespace DrMoradi.Areas.Admin.Controllers
{
    [Area("Api")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AloPeykWebhookController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
