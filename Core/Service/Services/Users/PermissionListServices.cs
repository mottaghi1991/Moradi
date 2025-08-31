using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Extention;
using Core.Interface.Admin;
using Core.Service.Interface.Admin;
using Dapper;
using Data;
using Data.MasterInterface;
using Domain;
using Domain.User;
using Domain.User.Permission;
using Fuel_Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Users
{
    public class PermissionListServices : IPermisionList
    {
        IMaster<PermissionList> _master;
        private readonly IRolePermission _rolePermission;
        private readonly IRole _Role;

        public PermissionListServices(IMaster<PermissionList> master, IRolePermission rolePermission, IRole role)
        {
            _master = master;

            _rolePermission = rolePermission;
            _Role = role;
        }
        #region Area,Controller,Action
        public async Task<IEnumerable<PermissionList>> GetAllAsync()
        {
            return await _master.GetAllEfAsync();
        }
        public async Task<IEnumerable<PermissionList>> GetPermisionByAreaAndControllerAsync(string Area, string Controller)
        {
            var obj = await _master.GetAllEfAsync();
            var result = obj
    .Where(sm =>
        (sm.Area == Area || sm.Area == null) &&
        (sm.ControllerName == Controller || Controller == null))
    .Select(sm => new PermissionList
    {
        Id = sm.Id,
        ParentId = sm.ParentId,
        Radif = sm.Radif,
        Descript = sm.Descript,
        Area = sm.Area,
        ControllerName = sm.ControllerName,
        ActionName = sm.ActionName,
        Status = sm.Status ?? -1
    })
    .ToList();
            return result;
            //DynamicParameters p = new DynamicParameters();
            //p.Add("Area", Area, DbType.String);
            //p.Add("Controller", Controller, DbType.String);
            //return _master.GetAll("SearchPermission", p);
        }
        public async Task<IEnumerable<SelectListItem>> GetAllAreaAsync()
        {
            var obj = await _master.GetAllEfAsync();
            return obj
     .Select(sm => sm.Area)
     .Distinct()
     .Select(area => new SelectListItem
     {
         Text = area,
         Value = area
     })
     .ToList();
            //return _master.GetAll("GetAllArea");
        }
        public async Task<IEnumerable<SelectListItem>> GetControllerByAreaAsync(string Area)
        {
            var obj = await _master.GetAllEfAsync();
            var controllerNames = obj
           .Where(sm => sm.Area == Area)
           .Select(sm => sm.ControllerName)
           .Distinct()
           .Select(name => new SelectListItem
           {
               Text = name,
               Value = name
           })
           .ToList();
            return controllerNames;
            //DynamicParameters p = new DynamicParameters();
            //p.Add("Area", Area, DbType.String);
            //return _master.GetAll("GetControllerByArea", p);

        }
        public async Task insertAreaAsync(IEnumerable<ControllerActions> ListOfControlerAndAction)
        {

            foreach (var item in ListOfControlerAndAction)
            {


                int AreaId = 0;
                if (item.Area != null)
                {
                    if (await checkExistAreaAsync(item.Area) == 0)
                    {
                        await InsertAsync(new PermissionList()
                        {
                            Area = item.Area,
                            ControllerName = null,
                            ActionName = null,
                            ParentId = 0,
                            Descript = null,
                            Status = (int)MenuStatus.permission
                        });

                        AreaId = await checkExistAreaAsync(item.Area);
                    }
                    else
                    {
                        AreaId = await checkExistAreaAsync(item.Area);
                    }
                }
                else
                {
                    AreaId = await checkExistAreaAsync(item.Area);
                }


                if (await checkExistControllerAsync(item.Area, item.Controller) == 0)
                {
                    var menu = new PermissionList()
                    {
                        Area = item.Area,
                        ControllerName = item.Controller,
                        ActionName = null,
                        ParentId = AreaId,
                        Descript = null,
                        Status = null

                    };
                    await InsertAsync(menu);
                    AreaId = await checkExistControllerAsync(item.Area, item.Controller);
                }
                else
                {
                    AreaId = await checkExistControllerAsync(item.Area, item.Controller);
                }
                if (await CheckExistPermissionAsync(item.Area, item.Controller, item.Action) != true)
                {
                    var menu = new PermissionList()
                    {
                        Area = item.Area,
                        ControllerName = item.Controller,
                        ActionName = item.Action,
                        ParentId = AreaId,
                        Descript = null,
                        Status = null

                    };
                    await InsertAsync(menu);
                }

            }
        }
        public async Task<int> checkExistAreaAsync(string Area)
        {
            var obj = await _master.GetAllEfAsync(a => a.Area == Area && a.Area != null && a.ActionName == null && a.ControllerName == null);
            if (obj.Any())
            {
                var menu = await _master.GetAllEfAsync(a => a.Area == Area && a.Area != null && a.ActionName == null && a.ControllerName == null);
                return menu.FirstOrDefault().Id;

            }
            else
                return 0;
        }
        public async Task<int> checkExistControllerAsync(string Area, string Controller)
        {
            var obj = await _master.GetAllEfAsync(a => a.Area == Area && a.ControllerName == Controller && a.ActionName == null);
            if (obj.Any())
            {
                var menu = await _master.GetAllEfAsync(a => a.Area == Area && a.ControllerName == Controller && a.ActionName == null);
                return menu.FirstOrDefault().Id;
            }

            else
                return 0;
        }
        public async Task<bool> CheckExistPermissionAsync(string Area, string Controller, string Action)
        {
            var t = await _master.GetAllEfAsync(a => a.Area == Area & a.ControllerName == Controller & a.ActionName == Action);
            return t.Any();
        }
        public async Task<IEnumerable<SelectListItem>> GetContrllersOfAreaAsync(int SystemMenuId)
        {
            var Allarea = await _master.GetAllEfAsync(a => a.ParentId == SystemMenuId);
            var area = Allarea.FirstOrDefault()?.Area;
            var allobj = await _master.GetAllEfAsync(a => a.Area == area & a.ActionName == null);
            var obj = allobj.Select(a => new SelectListItem()
            {
                Text = a.ControllerName,
                Value = a.Id.ToString()
            }).OrderBy(a => a.Text).AsEnumerable().Append(new SelectListItem("منو", "-1")).Append(new SelectListItem("کنترلر اصلی", "-2"));
            return obj;
        }

        public IList<ControllerActions> ActionAndControllerNamesList()
        {
            string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
            Assembly asm = Assembly.Load(assemblyName);
            var controlleractionlist = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new
                {
                    Controller = x.DeclaringType.Name,
                    Action = x.Name,
                    Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))

                }).ToList();
            var list = new List<ControllerActions>();
            foreach (var item in controlleractionlist)
            {
                if (item.Area.Count() != 0)
                {
                    list.Add(new ControllerActions()
                    {
                        Controller = item.Controller.Replace("Controller", null),
                        Action = item.Action,
                        Area = item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault()
                    });
                }
                else
                {
                    list.Add(new ControllerActions()
                    {
                        Controller = item.Controller.Replace("Controller", null),
                        Action = item.Action,
                        Area = null,

                    });
                }
            }

            return list;
        }
        #endregion




        public async Task<PermissionList> InsertAsync(PermissionList permissionList)
        {
            return await _master.InsertAsync(permissionList);
        }

        public async Task<PermissionList> GetByIdAsync(int PermissionId)
        {
            var obj = await _master.GetAllEfAsync(a => a.Id == PermissionId);
            return obj.FirstOrDefault();
        }

        public async Task<PermissionList> UpdateAsync(PermissionList permissionList)
        {
            return await _master.UpdateAsync(permissionList);
        }
        public async Task<bool> DeleteAsync(PermissionList permissionList)
        {
            return await _master.DeleteAsync(permissionList);
        }




        #region Permission

        public async Task<IEnumerable<SelectListItem>> PermissionHeadListAsync()
        {
            var all = await _master.GetAllEfAsync(a => a.ParentId == 0 & a.Status == (int)MenuStatus.permission);
            var obj = all.Select(a => new SelectListItem()
            {
                Text = a.Descript,
                Value = a.Id.ToString()
            });
            var dis = obj.OrderBy(a => a.Text);
            return dis;
        }
        public async Task<IEnumerable<PermissionList>> PermissionMiddListAsync()
        {
            var obj = await _master.GetAllEfAsync(a => a.ParentId == null && a.Status == (int)MenuStatus.permission);
            return obj;
        }


        public async Task<IEnumerable<PermissionList>> PermissionAllListAsync()
        {
            var obj = await _master.GetAllEfAsync(a => a.Status == (int)MenuStatus.permission);
            return obj;
        }
        #endregion


        #region Menu
        public async Task<IEnumerable<SelectListItem>> HeadMenuAsync()
        {
            var obj = await _master.GetAllEfAsync(a => a.ParentId == null);
            var re = obj.Select(a => new SelectListItem()
            {
                Text = a.Descript,
                Value = a.Id.ToString()
            });
            var dis = re.OrderBy(a => a.Text);
            return dis;
        }
        public async Task<PermissionList> InsertHeadMenuAsync(PermissionList permissionList)
        {
            PermissionList permission = new PermissionList()
            {
                Area = null,
                ParentId = null,
                Descript = permissionList.Descript,
                Radif = permissionList.Radif,
                Status = (int)MenuStatus.menu,
                ActionName = null,
                ControllerName = null,

            };
            var Result = await _master.InsertAsync(permission);
            return Result;
        }
        public async Task<IEnumerable<PermissionList>> GetAllMenuAsync()
        {
            return await _master.GetAllEfAsync(a => a.Status == (int)MenuStatus.menu);
        }
        #endregion


        public async Task<bool> CheckFirstAsync()
        {
            var result = await _master.GetAllEfAsync(null);

            if (!result.Any())
            {

                await _master.InsertAsync(new PermissionList()
                {
                    ParentId = 0,
                    Area = null,
                    Descript = "دسترسی ریشه بدون Area",
                    Radif = 1,
                    Status = (int)MenuStatus.permission

                });
                var Res = await _master.InsertAsync(new PermissionList()
                {
                    ParentId = null,
                    Area = null,
                    Descript = "دسترسی ها",
                    Radif = 1,
                    Status = (int)MenuStatus.menu
                });

                if (Res == null)
                {
                    return false;
                }
                return true;
            }

            return true;
        }


        #region User Access

        public async Task<IEnumerable<PermissionList>> GetPermissionOfUserAsync(int UserId)
        {
            var permissions = (from pl in await _master.GetAllEfAsync()
                               join rp in await _rolePermission.getallAsync() on pl.Id equals rp.PermissionListId
                               join ur in await _Role.GetAllUserRoleAsync() on rp.RoleId equals ur.RoleId
                               where ur.UserId == UserId
                               select pl).ToList();
            return permissions;
            //DynamicParameters p = new DynamicParameters();
            //p.Add("UserId", UserId, DbType.Int32);
            //return _master.GetAll("GetPermissionOfUser", p);
        }

        public async Task<bool> HasAccessAsync(Microsoft.AspNetCore.Http.HttpContext httpContext, string area, string controller, string action)
        {
            var permissions = httpContext.Session.GetData<IEnumerable<PermissionList>>("UserPermission");

            if (permissions == null)
            {
                permissions = await GetPermissionOfUserAsync(httpContext.User.GetUserId());
                httpContext.Session.SetData("UserPermission", permissions);
            }
            if (permissions == null)
            { return false; }
            var obj = permissions.Any(p =>
                (p.Area == area && p.ControllerName == null && p.ActionName == null) ||
                (p.Area == area && p.ControllerName == controller && p.ActionName == null) ||
                (p.Area == area && p.ControllerName == controller && p.ActionName == action)
            );
            return obj;
        }

        public async Task<bool> setPermissionsettingAsync(IEnumerable<PermissionList> permissionLists)
        {
            var firstobj = new PermissionList()
            {
                Radif = 1,
                Descript = "دسترسی ها",
                ParentId = null,
                Status = (int)MenuStatus.menu
            };
            var Firstmenu = await InsertAsync(firstobj);
            var obj = permissionLists.FirstOrDefault(a => a.Area == "Admin" & a.ControllerName == "PermissionList" & a.ActionName == "MenuList");
            obj.ParentId = Firstmenu.Id;
            obj.Status = (int)MenuStatus.menu;
            obj.Descript = "منو";
            await UpdateAsync(obj);
            //var permi=permissionLists.FirstOrDefault(a => a.Area == "Admin" & a.ControllerName == "PermissionList" & a.ActionName == "ShowPermision");
            //obj.ParentId = Firstmenu.Id;
            //obj.Status = (int)MenuStatus.menu;
            //obj.Descript = "مدیریت دسترسی ها";
            //Update(permi);
            //var ROlemenu = permissionLists.FirstOrDefault(a => a.Area == "Admin" & a.ControllerName == "Role" & a.ActionName == "Index");
            //obj.ParentId = Firstmenu.Id;
            //obj.Status = (int)MenuStatus.menu;
            //obj.Descript = "گروه ها";
            //Update(ROlemenu);
            return true;
        }
        public async Task<IEnumerable<PermissionList>> UserMenuAsync(int UserId)
        {
            return await _master.GetAllEfAsync(a => a.Status == (int)MenuStatus.menu);
            //DynamicParameters p = new DynamicParameters();
            //p.Add("UserId", UserId, DbType.Int32);
            //return _master.GetAll("UserMenu", p).ToList();
        }
        #endregion
    }
}

