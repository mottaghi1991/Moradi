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
using Core.Enums;

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
            return _master.GetAllAsQueryable(a => a.RoleId == RoleId && a.PermissionList.Status == (int)MenuStatus.menu)
                .Include(a => a.Role)
                .Include(a => a.PermissionList).ToList();

        }

        public async Task<IEnumerable<RolePermission>> GetPermissionOfRoleAsync(int RoleId)
        {

            return await _master.GetAllAsQueryable().Include(a => a.PermissionList).Where(a => a.RoleId == RoleId&&a.PermissionList.Status==(int)MenuStatus.permission).ToListAsync();
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

