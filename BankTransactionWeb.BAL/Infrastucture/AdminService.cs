using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
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
            try
            {
                var identityRole = new IdentityRole
                {
                    Name = role.Name
                };
                var result = unitOfWork.RoleManager.CreateAsync(identityRole);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IdentityResult> DeleteRole(string id)
        {
            try
            {

                var roleReturned = await unitOfWork.RoleManager.FindByIdAsync(id);
                if (roleReturned == null)
                    return null;
                var result = await unitOfWork.RoleManager.DeleteAsync(roleReturned);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IdentityResult> AddUserToRole(string id, bool isUserSelected, string roleName)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            try
            {
                return unitOfWork.RoleManager.Roles.Select(e => mapper.Map<RoleDTO>(e));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Dispose()
        {
            unitOfWork.Dispose();
        }
        public async Task<IEnumerable<PersonInRoleDTO>> GetAllUsersInCurrentRole(string id)
        {
            try
            {
                var identityRole = await unitOfWork.RoleManager.FindByIdAsync(id);
                if (identityRole == null)
                {
                    return null;
                }
                var resultList = new List<PersonInRoleDTO>();
                foreach (var user in await unitOfWork.PersonRepository.GetAll())
                {
                    var userInRole = mapper.Map<PersonInRoleDTO>(user);
                    if (userInRole == null || user.ApplicationUser == null)
                        continue;
                    if (await unitOfWork.UserManager.IsInRoleAsync(user.ApplicationUser, identityRole.Name))
                    {
                        userInRole.IsSelected = true;
                    }
                    else
                        userInRole.IsSelected = false;
                    resultList.Add(userInRole);
                }
                return resultList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<RoleDTO> GetRoleById(string id)
        {
            try
            {
                var identityRole = await unitOfWork.RoleManager.FindByIdAsync(id);
                if (identityRole == null)
                {
                    return null;
                }
                return mapper.Map<RoleDTO>(identityRole);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<RoleDTO> GetRoleWithUsers(string id)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError($"An error ocurred : {ex.Message}. Inner exeption : {ex.InnerException}");
                throw ex;

            }
        }

        public async Task<IdentityResult> UpdateRole(RoleDTO role)
        {
            try
            {
                var identityRole = await unitOfWork.RoleManager.FindByIdAsync(role.Id);
                identityRole.Name = role.Name;
                if (identityRole == null)
                {
                    return null;
                }
                var result = await unitOfWork.RoleManager.UpdateAsync(identityRole);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"An error ocurred : {ex.Message}. Inner exeption : {ex.InnerException}");
                throw ex;

            }
            
        }

    }
}
