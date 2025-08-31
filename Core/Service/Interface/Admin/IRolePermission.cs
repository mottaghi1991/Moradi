using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Domain.User.Permission;

namespace Core.Service.Interface.Admin
{
    public interface IRolePermission
    {
        Task<IEnumerable<RolePermission>> GetMenuOfRoleAsync(int RoleId);
        Task<IEnumerable<RolePermission>> GetPermissionOfRoleAsync(int RoleId);
        Task<IEnumerable<RolePermission>> getallAsync();
        Task<bool> BulkInsertAsync(List<RolePermission> list);

        Task<bool> BulkDeleteAsync(List<RolePermission> list);
    }
}