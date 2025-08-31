using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Interface.Users;
using Core.Service.Interface.Admin;
using Data.MasterInterface;
using Domain.User.Permission;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Core.Service.Services.Users
{
    public class RolePermissionServices : IRolePermission
    {
        private IMaster<RolePermission> _master;


        public RolePermissionServices(IMaster<RolePermission> master)
        {
            _master = master;
        }
        public async Task<IEnumerable<RolePermission>> GetMenuOfRoleAsync(int RoleId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("RoleId", RoleId, System.Data.DbType.Int32);

            return await _master.GetAllAsync("GetMenuOfRole", p);
        }

        public async Task<IEnumerable<RolePermission>> GetPermissionOfRoleAsync(int RoleId)
        {

            return await _master.GetAllAsQueryable().Include(a => a.PermissionList).Where(a => a.RoleId == RoleId).ToListAsync();


            //.Select(a => new PermissionList
            //{
            //    Area = a.PermissionList.Area,
            //    ActionName = a.PermissionList.ActionName,
            //    ControllerName = a.PermissionList.ControllerName,
            //    PermissionListId = a.PermissionList.PermissionListId,
            //    ParentId = a.PermissionList.ParentId,
            //    Descript = a.PermissionList.Descript,
            //    Radif = a.PermissionList.Radif,
            //    Status = a.PermissionList.Status,
            //}).ToList();
        }

        public async Task<bool> BulkInsertAsync(List<RolePermission> list)
        {
            return await _master.BulkeInsertAsync(list);
        }

        public async Task<bool> BulkDeleteAsync(List<RolePermission> list)
        {
            return await _master.BulkeDeleteAsync(list);
        }

        public async Task<IEnumerable<RolePermission>> getallAsync()
        {
            return await _master.GetAllEfAsync();
        }
    }
}

