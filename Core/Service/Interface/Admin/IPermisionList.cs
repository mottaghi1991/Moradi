using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.User;
using Domain.User.Permission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Interface.Admin
{
    public interface IPermisionList
    {
        #region Area,Controller,Action
       Task<IEnumerable<PermissionList>> GetAllAsync();
        Task<IEnumerable<PermissionList>> GetPermisionByAreaAndControllerAsync(string Area, string Controller);
        Task<IEnumerable<SelectListItem>> GetAllAreaAsync();
        Task<IEnumerable<SelectListItem>> GetControllerByAreaAsync(string Area);
        Task<int> checkExistAreaAsync(string Area);
        Task<int> checkExistControllerAsync(string Area, string Controller);
        Task<bool> CheckExistPermissionAsync(string Area, string Controller, string Action);
         IList<ControllerActions> ActionAndControllerNamesList();
         Task insertAreaAsync(IEnumerable<ControllerActions> ListOfControlerAndAction);
         Task<IEnumerable<SelectListItem>> GetContrllersOfAreaAsync(int SystemMenuId);
        #endregion




        Task<PermissionList> InsertAsync(PermissionList permissionList);
        Task<PermissionList> GetByIdAsync(int PermissionId);
        Task<PermissionList> UpdateAsync(PermissionList permissionList);
       Task<bool> DeleteAsync(PermissionList permissionList);




        #region Menu

        Task<IEnumerable<SelectListItem>> HeadMenuAsync();
        Task<PermissionList> InsertHeadMenuAsync(PermissionList permissionList);
         Task<IEnumerable<PermissionList>> GetAllMenuAsync();
        #endregion
        #region Permission
        Task<IEnumerable<SelectListItem>> PermissionHeadListAsync();
        Task<IEnumerable<PermissionList>> PermissionMiddListAsync();
        Task<IEnumerable<PermissionList>> PermissionAllListAsync();

        #endregion




        Task<bool> CheckFirstAsync();

        #region User Access
         Task<IEnumerable<PermissionList>> GetPermissionOfUserAsync(int UserId);
         Task<bool> HasAccessAsync(HttpContext httpContext, string area, string controller, string action);
        Task<bool> setPermissionsettingAsync(IEnumerable<PermissionList> permissionLists);
        Task<IEnumerable<PermissionList>> UserMenuAsync(int UserId);
        #endregion
    }
}
