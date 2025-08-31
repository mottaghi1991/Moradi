using System;
using Core.Dto.ViewModel.Admin.Role;
using System.Collections.Generic;
using System.Linq;
using Domain.User;
using Domain.User.Permission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Service.Interface.Admin;
using Core.Service.Interface.Users;
using WebStore.Base;
using Core.Interface.Admin;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace PersonalSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private IRole _role;
        private readonly IUser _user;
        private readonly IRolePermission _permission;
        private readonly IPermisionList _permisionList;

        public RoleController(IRole role, IRolePermission permission, IPermisionList permisionList, IUser user)
        {
            _role = role;
            _permission = permission;
            _permisionList = permisionList;
            _user = user;
        }
        // GET: RoleController
        public async Task<ActionResult> Index()
        {
            var obj =await _role.GetAllRoleAsync();
            return View(obj);
        }


        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role Role)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return new JsonResult(Role);
                }
            }
            catch
            {
                return new JsonResult(Role);
            }

           await _role.insertAsync(Role);
            return RedirectToAction("Index");
        }

        // GET: RoleController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var result =await _role.GetRoleAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Role role)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(role);
            }

           await _role.updateAsync(role);
            return RedirectToAction("Index");
        }

        // GET: RoleController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {


            if (id == null)
            {
                return BadRequest();
            }
            var result =await _role.GetRoleAsync(id);
            if (result == null)
            {
                return NotFound();
            }

           await _role.deleteAsync(result);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> UserRole(int RoleId)
        {
            var Role =await _role.GetRoleAsync(RoleId);
            if (Role == null)
            {
                return BadRequest();
            }
            ViewBag.AdminUser =await _user.GetAllAdminAsync();
            var obj = new EditUserRoleVm()
            {
                RoleId = RoleId,
                RoleName = Role.RoleTitle,
                UserRoles =await _role.GetAllUSerOfRoleAsync(RoleId)
            };
            return View(obj);
        }

        [HttpPost]
        public async  Task<IActionResult> UserRole(List<int> UserList, int RoleId)
        {
            var deletedata =await _role.GetAllUSerOfRoleAsync(RoleId);
            if (deletedata.Any())
            {
                if (await _role.BulkDeleteAsync(deletedata) != null)
                {
                    TempData["Error"] = "خطایی رخ داده است";
                    return RedirectToAction("UserRole", new { RoleId = RoleId });
                }


            }
            List<UserRole> list = new List<UserRole>();
            foreach (int i in UserList)
            {
                list.Add(new UserRole()
                {
                    RoleId = RoleId,
                    UserId = i
                });
            }
            list.AddRange(list);

            var obj =await _role.BulkInsertAsync(list);
            if (obj)
            {
                TempData["Success"] = "عملیات با موفقیت انجام گردید";
                return RedirectToAction("Index", "Role");
            }
            else
            {
                TempData["Error"] = "خطایی رخ داده است";
                return RedirectToAction("Index", "Role");
            }


        }
        [HttpGet]
        public async Task<IActionResult> RolePermission(int RoleId)
        {
            var deletedata =await _permission.GetPermissionOfRoleAsync(RoleId);
            var Role =await _role.GetRoleAsync(RoleId);
            if (Role == null)
            {
                return BadRequest();
            }
            ViewBag.PermssionList =await _permisionList.PermissionAllListAsync();
            var obj = new EditPermissionRoleVm()
            {
                RoleId = RoleId,
                RoleName = Role.RoleTitle,
                MenuVm =await _permission.GetPermissionOfRoleAsync(RoleId)
            };
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> RolePermission(List<int> PermissionList, int RoleId)
        {
            var deletedata =await _permission.GetPermissionOfRoleAsync(RoleId);
            if (deletedata.Any())
            {
                if (await _permission.BulkDeleteAsync(deletedata.ToList())!=true)
                {
                    TempData["Error"] = "خطایی رخ داده است";
                    return RedirectToAction("RolePermission", new { RoleId = RoleId });
                }


            }
            List<RolePermission> list = new List<RolePermission>();
            foreach (int i in PermissionList)
            {
                list.Add(new RolePermission()
                {
                    RoleId = RoleId,
                    PermissionListId = i
                });
            }
            list.AddRange(list);

            var obj = await _permission.BulkInsertAsync(list);
            if (obj)
            {
                TempData["Success"] = "عملیات با موفقیت انجام گردید";
                return RedirectToAction("Index", "Role");
            }
            else
            {
                TempData["Error"] = "خطایی رخ داده است";
                return RedirectToAction("Index", "Role");
            }
        }
        public async Task<IActionResult> RoleMenu(int RoleId)
        {
            var Role = await _role.GetRoleAsync(RoleId);
            if (Role == null)
            {
                return BadRequest();
            }
            ViewBag.PermssionList =await _permisionList.GetAllMenuAsync();
            var obj = new EditPermissionRoleVm()
            {
                RoleId = RoleId,
                RoleName = Role.RoleTitle,
                MenuVm =await _permission.GetMenuOfRoleAsync(RoleId)
            };
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> RoleMenu(List<int> PermissionList, int RoleId)
        {
            var deletedata =await _permission.GetMenuOfRoleAsync(RoleId);
            if (deletedata.Any())
            {
                if (await  _permission.BulkDeleteAsync(deletedata.ToList())!=true)
                {
                    TempData["Error"] = "خطایی رخ داده است";
                    return RedirectToAction("RolePermission", new { RoleId = RoleId });
                }

            }
            List<RolePermission> list = new List<RolePermission>();
            foreach (int i in PermissionList)
            {
                list.Add(new RolePermission()
                {
                    RoleId = RoleId,
                    PermissionListId = i
                });
            }
            list.AddRange(list);

            var obj =await _permission.BulkInsertAsync(list);
            if (obj)
            {
                TempData["Success"] = "عملیات با موفقیت انجام گردید";
                return RedirectToAction("Index", "Role");
            }
            else
            {
                TempData["Error"] = "خطایی رخ داده است";
                return RedirectToAction("Index", "Role");
            }
        }
    }
}
