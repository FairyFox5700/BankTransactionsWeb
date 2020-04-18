using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IAdminService:IDisposable
    {
        Task<IdentityResult> AddUserToRole(string id, bool isUserSelected, string roleName);
        Task<IdentityResult> AddRole(RoleDTO role);
        IEnumerable<RoleDTO> GetAllRoles();
        Task<IEnumerable<PersonInRoleDTO>> GetAllUsersInCurrentRole(string id);
        Task<RoleDTO> GetRoleById(string id);
        Task<RoleDTO> GetRoleWithUsers(string id);
        Task<IdentityResult> UpdateRole(RoleDTO role);
        Task<IdentityResult> DeleteRole(string id);
    }
}
