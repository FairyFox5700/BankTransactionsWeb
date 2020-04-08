using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface IAdminService
    {
        Task<IdentityResult> AddUserToRole(string id, bool isUserSelected, string roleName);
        Task<IdentityResult> AddRole(RoleDTO role);
        IEnumerable<RoleDTO> GetAllRoles();
        Task<IEnumerable<PersonInRoleDTO>> GetAllUsersInCurrentRole(string id);
        Task<RoleDTO> GetRoleById(string id);
        Task<RoleDTO> GetRoleWithUsers(string id);
        Task<IdentityResult> UpdateRole(string id);
    }
}
