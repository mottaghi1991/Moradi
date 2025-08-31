using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Dto.ViewModel.Admin.Role;
using Core.Service.Interface.Admin;
using Core.Service.Interface.Users;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace PersonalSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly Core.Service.Interface.Users.IUser _user;
        private readonly IRole _role;

        public UserController(Core.Service.Interface.Users.IUser user, IRole role)
        {
            _user = user;
            _role = role;
        }
        public async Task<IActionResult> Index(int pageid = 1)
        {
            var obj =await _user.GetPaggingUserAsync(1, 10);
            return View(obj);
        }

    }
}
