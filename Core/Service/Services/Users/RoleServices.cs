using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Service.Interface.Admin;
using Data.MasterInterface;
using Domain.User;
using Domain.User.Permission;

namespace Core.Service.Services.Users
{
    public class RoleServices : IRole
    {
        private readonly IMaster<Role> _master;
        private readonly IMaster<UserRole> _UserRolemaster;

        public RoleServices(IMaster<Role> master, IMaster<UserRole> userRolemaster)
        {
            _master = master;
            _UserRolemaster = userRolemaster;
        }
        public Task<IEnumerable<Role>> GetAllRoleAsync()
        {
            return _master.GetAllEfAsync(null);
        }

        public async Task<Role> GetRoleAsync(int RoleId)
        {
            var obj =await _master.GetAllEfAsync(a => a.RoleId == RoleId);
            return obj.FirstOrDefault();
        }

        public async Task<Role> insertAsync(Role role)
        {
            return await _master.InsertAsync(role);
        }

        public async Task<Role> updateAsync(Role role)
        {
            return await _master.UpdateAsync(role);
        }

        public async Task<bool> deleteAsync(Role role)
        {
            return await _master.DeleteAsync(role);
        }

        public Task<IEnumerable<UserRole>> GetAllUSerOfRoleAsync(int RoleId)
        {
            return _UserRolemaster.GetAllEfAsync(a => a.RoleId == RoleId);
        }

        public async Task<bool> BulkDeleteAsync(IEnumerable<UserRole> List)
        {
            return await _UserRolemaster.BulkeDeleteAsync(List);
        }

        public async Task<bool> BulkInsertAsync(IEnumerable<UserRole> List)
        {
            return await  _UserRolemaster.BulkeInsertAsync(List.ToList());
        }

        public async Task<IEnumerable<UserRole>> GetAllUserRoleAsync()
        {
           return await _UserRolemaster.GetAllEfAsync();
        }

        public async Task<bool> UserRoleInsertAsync(UserRole userRole)
        {
            var result =await _UserRolemaster.InsertAsync(userRole);
            return result == null ? false : true;
        }
    }
}
