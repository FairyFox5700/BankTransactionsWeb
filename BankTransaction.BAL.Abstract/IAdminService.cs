using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.BAL.Abstract
{
    public interface IAdminService:IDisposable
    {
        Task<IdentityUserResult> AddUserToRole(string id, bool isUserSelected, string roleName);
        Task<IdentityUserResult> AddRole(RoleDTO role);
        IEnumerable<RoleDTO> GetAllRoles();
        Task<IEnumerable<PersonInRoleDTO>> GetAllUsersInCurrentRole(string id);
        Task<RoleDTO> GetRoleById(string id);
        Task<RoleDTO> GetRoleWithUsers(string id);
        Task<IdentityUserResult> UpdateRole(RoleDTO role);
        Task<IdentityUserResult> DeleteRole(string id);
    }
}
