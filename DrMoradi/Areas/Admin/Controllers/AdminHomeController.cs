using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace PersonalSite.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminHomeController : Controller
    {
        private readonly IDistributedCache _cache;

        public AdminHomeController(IDistributedCache cache)
        {
            _cache = cache;
        }
        [Route("Admin")]
        public IActionResult Index()
        {

            return RedirectToAction("Index","DietOrder");
        }


    }
}
