using Core.Dto.ViewModel.Dr.DietVm;
using Core.Dto.ViewModel.main;
using Core.Dto.ViewModel.User;
using Core.Extention;
using Core.Service.Interface.Admin;
using Core.Service.Interface.Dr;
using Core.Service.Interface.MainPage;
using Core.Service.Interface.Users;
using Domain;
using Domain.Dr;
using Domain.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebStore.Base;

namespace DrMoradi.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISetting _setting;
        private readonly IDiet _diet;
        private readonly IComment _comment;
        private readonly IUser _user; 

        private IViewRender _viewRender;

        public HomeController(ILogger<HomeController> logger, ISetting setting, IViewRender viewRender, IDiet diet, IComment comment, IUser user)
        {
            _logger = logger;
            _setting = setting;

            _viewRender = viewRender;
            _diet = diet;
            _comment = comment;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            //var assemblyName = typeof(HomeController).Assembly.FullName;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/Error")]
        public IActionResult Error(Exception exception)
        {
            System.Diagnostics.Activity? CurrentActivity = System.Diagnostics.Activity.Current;
            string TraceId = HttpContext.TraceIdentifier;
            string path = HttpContext.Request.Path;
            //  _logger.LogError(EventIdList.Error, "(Exceptions)User={UserId} Request={RequestPath} with TraceId={TraceId} get Exception", User.GetUserId(), path, TraceId);

            return View(new ErrorViewModel()
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,

            });
        }
        [HttpGet]
        public async Task<IActionResult> DietDetail(int DietId)
        {
            var diet = await _diet.GetDietById(DietId);
            if(diet==null)
            {
                return NotFound();
            }
            var obj = new ShowDietDetailVm()
            {
                diet = diet,
                comments = await _comment.GetCommentsByDietId(DietId),
            };
            return View(obj);
        }
        [Route("/NotFound")]
        public IActionResult NotFound(string Path)
        {

            _logger.LogWarning(eventId: EventIdList.NotFound, "(Not Found)UserId={UserId} with Path={Path} ", User.GetUserId(), Path);
            return View();
        }
        [HttpGet]
        [Route("/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        [Route("/Contact")]
        public async Task<IActionResult> Contact()
        {
            return View();
        }


        [Route("/Diets")]
        public async Task<IActionResult> Diets()
        {
            return View(await _diet.GetAllAsync());
        }
        [Route("/BMI")]
        [HttpGet]
        public async Task<IActionResult> BMI()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BMI( BMI model)
        {
            if (!ModelState.IsValid)
            {


                // پاسخ خطا به صورت JSON و با وضعیت 400
                return BadRequest(new { Success = false, Message = "اطلاعات کامل وارد نشده." });
            }
            double ideal, health = 0;
            if (model.Gender == Gender.man)
            {
                ideal = (48.1 + 1.1 * (model.Height - 152));
            }
            else
            {
                ideal = (45.5 + 0.9 * (model.Height - 152));
            }
            health = ideal;
            health = health * 1.10;
            switch (model.Stone)
            {
                case Stone.dorosht:
                    health = ideal * 1.10;
                    break;
                case Stone.riz:
                    health = ideal * 0.90;
               
                    break;
                case Stone.motevaset:
                    
                    break;

            }
            if (model.Age > 50)
            {
                ideal *= 1.10;
                health *= 1.10;
            }
            double bmi = model.Weight / Math.Pow(model.Height / 100.0, 2);
            string weightStatus=null,color=null;
            if(model.Weight<ideal)
            {
                weightStatus = "شما کمبود وزن دارید";
                color = "#76a9ff"; // آبی
            }
            else if (ideal<=model.Weight &&model.Weight<=health)
            {
                weightStatus = "شما وزن نرمال دارید";
                color = "#78f9a6"; // سبز
            }
            else if (model.Weight>health)
            {
                weightStatus = "شما اضافه وزن دارید";
                color = "#f9be78"; // نارنجی
            }
          
            var result = new
            {
                BMIValue = Math.Round(bmi, 2),
                WeightStatus = weightStatus,
                IdealWeight = Math.Round(ideal, 1),
                HealthyWeight = Math.Round(health, 1),
                Description = "...",
                AdviceTitle = "پیشنهاد ما برای شما",
                AdviceDetail = "...",
                ButtonText = "مشاهده رژیم‌های غذایی",
                DangerText = weightStatus.Contains("چاق") || weightStatus.Contains("اضافه") ? "شما اضافه وزن دارید" : "",
                Color = color ,// رنگ رو اضافه کردیم,
                showadvice = weightStatus != "نرمال"
            };

            return Json(result);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsualComment(UsualComentVM comment)
        {
            if (!ModelState.IsValid)
            {
                comment.status = "false";
                return ViewComponent("CommentComponent", new { model = comment });
            }
            var result = await _comment.Insert(new Comment()
            {
                CreateDate = DateTime.Now,
                EntityType = EntityType.Home,
                IsApproved = false,
                IsDeleted = false,
                Mobile = comment.Mobile,
                Name = comment.Name,
                Text = comment.Text,

            });
            if (result)
            {
                comment.status = "true";
                return ViewComponent("CommentComponent", new { model = comment });

            }

            comment.status = "false";
            return ViewComponent("CommentComponent", new { model = comment });
        }
        [HttpPost]
        public async Task<IActionResult> SubmitComment(string message, int Id)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return Json(new { success = false, message = "پیام نمی‌تواند خالی باشد" });
            }

            try
            {
                var result = await _comment.Insert(new Comment()
                {
                    CreateDate = DateTime.Now,
                    EntityType = EntityType.Diet,
                    EntityId = Id,
                    IsApproved = false,
                    UserId = User.GetUserId(),
                    Text = message,


                });
                if (result)
                    return Json(new { success = true, message = "پیام شما با موفقیت ثبت شد" });
                else
                    return Json(new { success = false, message = "خطایی در ثبت پیام رخ داد" });

            }
            catch
            {
                return Json(new { success = false, message = "خطایی در ثبت پیام رخ داد" });
            }
        }
        [HttpGet]
        [Route("Rules")]
        public IActionResult Rules()
        {
            return View();
        }
        [Route("About")]
        public IActionResult About()
        {
            return View();
        }
      

    }
}
