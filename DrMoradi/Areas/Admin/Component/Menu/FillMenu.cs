using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Extention;
using Fuel_Core;
using Microsoft.AspNetCore.Authorization;
using Domain.User;
using Domain.User.Permission;
using Core.Service.Interface.Admin;
using Core.Service.Interface.Users;
using Core.Interface.Admin;

namespace PersonalSite.Areas.Admin.Component.Menu
{
    [Authorize]
    public class FillMenu : ViewComponent
    {
        private IPermisionList _permisionList;
        private IUser _user;

        public FillMenu(IPermisionList permisionList, IUser user)
        {
            _permisionList = permisionList;
            _user = user;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            IEnumerable<PermissionList> menus = new List<PermissionList>();
            if (HttpContext.Session.GetString("menus") == null)
            {

                var Identity = HttpContext.User.GetUserId();
                menus =await _permisionList.UserMenuAsync(Identity);
                HttpContext.Session.SetData("menus", menus);
            }
            else
            {
                menus = HttpContext.Session.GetData<List<PermissionList>>("menus");
            }



            return View("/Areas/Admin/Component/Menu/MyFillMenu.cshtml", menus);
        }
    }
}
