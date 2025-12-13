using AutoMapper;
using Core.Dto.ViewModel.User;
using Core.Service.Interface.Users;
using Microsoft.AspNetCore.Mvc;
using Core.Extention;
using WebStore.Base;
namespace PersonalSite.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminHomeController : BaseController
    {
        private readonly IUser _User;
        private readonly IMapper _mapper;
        public AdminHomeController(IUser user, IMapper mapper)
        {
            _User = user;
            _mapper = mapper;
        }
        [Route("Admin")]
        public async Task<IActionResult> Index()
        {
            return View();
            return RedirectToAction("Index","DietOrder");
        }
        public async Task<IActionResult> AdminList()
        {
            return View(await _User.GetAllAdminAsync());

        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string UserName)
        {
            var obj = await _User.GetUserByUserNameAsync(UserName);
            if (obj is null) return NotFound();
            return View(_mapper.Map<RegisterViewModel>(obj));
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var obj = await _User.GetUserByUserNameAsync(model.UserName);
            obj.PassWord= PasswordHelper.EncodePasswordMD5(model.PassWord);
            var Result=await _User.UpdateAsync(obj);
            if(Result!=null)
            {
                TempData[Success]=SuccessMessage;
                return RedirectToAction("AdminList");
            }
            TempData[Error]=ErrorMessage;
            return View(model);

        }


    }
}
