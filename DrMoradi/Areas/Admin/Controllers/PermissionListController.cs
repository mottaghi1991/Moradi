using Data.MasterInterface;
using Domain.User.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Enums;
using Core.Extention;
using Domain;
using Domain.User;
using Microsoft.Extensions.Logging;
using EventId = Domain.EventIdList;
using Core.Service.Interface.Admin;
using WebStore.Base;
using Core.Interface.Admin;

namespace PersonalSite.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class PermissionListController : BaseController
    {
        private IPermisionList _permisionList;
                
        private ILogger _logger;

        public PermissionListController(IPermisionList permisionList, ILogger<PermissionListController> logger)
        {
            _permisionList = permisionList;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> ShowPermision(string MyArea, string SearchController)
        {
            var obj1 = await _permisionList.GetAllAsync();
            if (!obj1.Any())
            {
                return RedirectToAction("insertArea");
            }
            if (MyArea == null)
            {
                ViewBag.Area = new SelectList(await _permisionList.GetAllAreaAsync(), "Value", "Text");
                ViewBag.Controller = new SelectList(await _permisionList.GetControllerByAreaAsync(MyArea), "Value", "Text", SearchController);
                return View(await _permisionList.GetAllAsync());
            }
           
            ViewBag.Area = new SelectList(await _permisionList.GetAllAreaAsync(), "Value", "Text", MyArea);
            ViewBag.Controller = new SelectList(await _permisionList.GetControllerByAreaAsync(MyArea), "Value", "Text", SearchController);
            var obj =await _permisionList.GetPermisionByAreaAndControllerAsync(MyArea, SearchController);
            return View(obj);


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var result =await _permisionList.GetByIdAsync(Id);
            if (result == null)
            {
                _logger.LogWarning(EventId.NotFound, "User With UserId={UserId} search Id={ItemId} and not Found ", User.GetUserId(), Id);
                return NotFound();
            }

            if (result.Status == (int)MenuStatus.menu)
            {
                ViewBag.Parent = new SelectList(await _permisionList.HeadMenuAsync(), "Value", "Text", result.ParentId);
            }
            else if (result.ParentId == null)
            {
                ViewBag.Parent = new SelectList(await _permisionList.PermissionHeadListAsync(), "Value", "Text", result.ParentId);
            }
            else if (result.Status == (int)MenuStatus.permission)
            {

                ViewBag.Parent = new SelectList(await _permisionList.PermissionHeadListAsync(), "Value", "Text", result.ParentId);
                ViewBag.Controller = new SelectList(await _permisionList.GetContrllersOfAreaAsync(result.ParentId.Value), "Value", "Text", result.ParentId.Value);
            }
            else if (result.Status == null)
            {
                ViewBag.Parent = new SelectList(await _permisionList.PermissionHeadListAsync(), "Value", "Text", result.ParentId);
                ViewBag.Controller = new SelectList( await _permisionList.GetContrllersOfAreaAsync(result.ParentId.Value), "Value", "Text", result.Id.ToString());
            }


            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PermissionList permissionList)
        {
            if (!ModelState.IsValid)
            {
                return View(permissionList);
            }
            var obj =await _permisionList.GetByIdAsync(permissionList.Id);
            obj.Radif = permissionList.Radif;
            obj.Descript = permissionList.Descript;
            // head menu
            if (obj.ParentId == null)
            {


            }
            //head permission
            if (obj.ParentId == 0)
            {

            }
            //laye 2 menu
            if (permissionList.Status == (int)MenuStatus.menu & obj.ParentId != null)
            {
                obj.ParentId = permissionList.ParentId != -1 ? permissionList.ParentId : obj.ParentId;
                obj.Status = permissionList.Status;
            }
            //system
            if (permissionList.Status == (int)MenuStatus.system)
            {
                obj.Status = permissionList.Status;
            }
            //head permission nist
            if (permissionList.Status == (int)MenuStatus.permission & permissionList.ParentId != 0)
            {
                //اگر دسترسی در سطح میانی بود دسترسی والد از parent گرفته شود
                if (permissionList.ControllerName == "-2")
                {
                    obj.ParentId = permissionList.ParentId;

                }
                //اگر دسترسی در سطح برگ بود parent از کنترلر گرفته شود

                else
                {
                    obj.ParentId = Convert.ToInt32(permissionList.ControllerName);
                }

                obj.Status = permissionList.Status;
            }
            //obj.ParentId = permissionList.ParentId != -1 ? permissionList.ParentId : obj.ParentId;
            //obj.Radif = permissionList.Radif;
            //obj.Descript = permissionList.Descript;
            //obj.Status = permissionList.Status;

            var Result =await _permisionList.UpdateAsync(obj);
            if (Result != null)
            {
                TempData[Success] = "عملیات با موفقیت ثبت گردید";
                return RedirectToAction("ShowPermision", new { MyArea = Result.Area, SearchController = Result.ControllerName });
            }

            TempData[Error] = "عملیات با خطا مواجه شد .";
            return RedirectToAction("ShowPermision", new { MyArea = obj.Area, SearchController = obj.ControllerName });

        }



        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null)
            {
                return BadRequest();
            }
            var result =await _permisionList.GetByIdAsync(Id);
            if (result == null)
            {
                return NotFound();
            }

           await _permisionList.DeleteAsync(result);
            return RedirectToAction("ShowPermision");
        }




        public async Task<IActionResult> insertArea()
        {
            var ali = _permisionList.ActionAndControllerNamesList();
           await _permisionList.insertAreaAsync(ali);
            //await  _permisionList.setPermissionsettingAsync(await _permisionList.GetAllAsync());
            return RedirectToAction("ShowPermision");
        }

        [HttpGet]
        public async Task<JsonResult> FillParent(int Id)
        {
            if (Id == (int)MenuStatus.permission)
            {
                var result = new SelectList(await _permisionList.PermissionHeadListAsync(), "Value", "Text");

                return Json(result);
            }
            else
            {
                var result = new SelectList(await _permisionList.HeadMenuAsync(), "Value", "Text");

                return Json(result);
            }

        }
        public async Task<JsonResult> FillController(int Id)
        {
            var result = new SelectList(await _permisionList.GetContrllersOfAreaAsync(Id), "Value", "Text");

            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> GetController(string MyArea)
        {
            var obj = new SelectList(await _permisionList.GetControllerByAreaAsync(MyArea), "Value", "Text");
            return Json(obj);
        }

        public async Task<IActionResult> MenuList()
        {
            ViewBag.MenuList = new SelectList(await _permisionList.GetAllMenuAsync(), "Id", "Descript");

            var obj =await _permisionList.GetAllMenuAsync();
            return View(obj);
        }
        [HttpGet]
        public IActionResult AddParentMenu()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddParentMenu(PermissionList permissionList)
        {
            if (!ModelState.IsValid)
            {
                return View(permissionList);
            }

            var result =await _permisionList.InsertHeadMenuAsync(permissionList);

            if (result != null)
            {
                TempData[Success] = "عملیات با موفقیت ثبت گردید";
                return RedirectToAction("MenuList", new { ParentMenuId = result.Id });
            }

            TempData[Error] = "عملیات با خطا مواجه شد .";
            return RedirectToAction("MenuList", new { ParentMenuId = result.Id });
        }
    }
}
