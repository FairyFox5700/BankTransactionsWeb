using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class AdminService : IAdminService
    {
        private readonly ILogger<AdminService> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AdminService(ILogger<AdminService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public Task<IdentityResult> AddRole(RoleDTO role)
        {
            var identityRole = new ApplicationRole
            {
                Name = role.Name
            };
            var result = unitOfWork.RoleManager.CreateAsync(identityRole);
            return result;
        }

        public async Task<IdentityResult> AddUserToRole(string id, bool isUserSelected, string roleName)
        {
            var user = await unitOfWork.UserManager.FindByIdAsync(id);
            if (user == null)
                return null;
            if (isUserSelected && !(await unitOfWork.UserManager.IsInRoleAsync(user, roleName)))
            {
                return await unitOfWork.UserManager.AddToRoleAsync(user, roleName);
            }
            else if (!isUserSelected && (await unitOfWork.UserManager.IsInRoleAsync(user, roleName)))
            {
                return await unitOfWork.UserManager.RemoveFromRoleAsync(user, roleName);
            }
            return null;
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            return unitOfWork.RoleManager.Roles.Select(e => new RoleDTO { Name = e.Name });
        }

        public async Task<IEnumerable<PersonInRoleDTO>> GetAllUsersInCurrentRole(string id)
        {
            var identityRole = await unitOfWork.RoleManager.FindByIdAsync(id);
            if (identityRole == null)
            {
                return null;
            }
            var resultList = new List<PersonInRoleDTO>();
            foreach (var user in unitOfWork.UserManager.Users)
            {
                var userInRole = mapper.Map<PersonInRoleDTO>(user);
                if (await unitOfWork.UserManager.IsInRoleAsync(user, identityRole.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                    userInRole.IsSelected = false;
                resultList.Add(userInRole);
            }
            return resultList;

        }

        public async Task<RoleDTO> GetRoleById(string id)
        {
            var identityRole = await unitOfWork.RoleManager.FindByIdAsync(id);
            if (identityRole == null)
            {
                return null;
            }
            return mapper.Map<RoleDTO>(identityRole);
        }


        public async Task<RoleDTO> GetRoleWithUsers(string id)
        {
            var identityRole = await GetRoleById(id);
            if (identityRole == null)
            {
                return null;
            }
            var roleMapped = mapper.Map<RoleDTO>(identityRole);
            {
                foreach (var user in unitOfWork.UserManager.Users)
                {
                    if (await unitOfWork.UserManager.IsInRoleAsync(user, identityRole.Name))
                    {
                        roleMapped.Users.Add(user.UserName);
                    }
                }
            }

            return roleMapped;
        }

        public async Task<IdentityResult> UpdateRole(string id)
        {
            var identityRole = await unitOfWork.RoleManager.FindByIdAsync(id);
            if (identityRole == null)
            {
                return null;
            }
            var result = await unitOfWork.RoleManager.UpdateAsync(identityRole);
            return result;
        }



    }
}
