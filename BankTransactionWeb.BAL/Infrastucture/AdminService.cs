using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.DAL.Abstract;
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AdminService( IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IdentityUserResult> AddRole(RoleDTO role)
        {
            var identityRole = new IdentityRole
            {
                Name = role.Name
            };
            var result = await unitOfWork.RoleManager.CreateAsync(identityRole);
            return result.Succeeded ? IdentityUserResult.SUCCESS : IdentityUserResult.GenerateErrorResponce(result);
        }


        public async Task<IdentityUserResult> DeleteRole(string id)
        {
            var roleReturned = await unitOfWork.RoleManager.FindByIdAsync(id);
            if (roleReturned == null)
                return new IdentityUserResult(){NotFound = true,Errors = new List<string>(){$"Role with {id} not found"}};
            var result = await unitOfWork.RoleManager.DeleteAsync(roleReturned);
            return result.Succeeded ? IdentityUserResult.SUCCESS : IdentityUserResult.GenerateErrorResponce(result);
        }
        public async Task<IdentityUserResult> AddUserToRole(string id, bool isUserSelected, string roleName)
        {
                var user = await unitOfWork.UserManager.FindByIdAsync(id);
                if (user == null)
                    return new IdentityUserResult(){NotFound = true,Errors = new List<string>(){$"User with {id}  not found"}};
                if (isUserSelected && !(await unitOfWork.UserManager.IsInRoleAsync(user, roleName)))
                {
                     var result =await unitOfWork.UserManager.AddToRoleAsync(user, roleName);
                     return result.Succeeded ? IdentityUserResult.SUCCESS : IdentityUserResult.GenerateErrorResponce(result);
                }
                else if (!isUserSelected && (await unitOfWork.UserManager.IsInRoleAsync(user, roleName)))
                {
                    var result = await unitOfWork.UserManager.RemoveFromRoleAsync(user, roleName);
                    return result.Succeeded
                        ? IdentityUserResult.SUCCESS
                        : IdentityUserResult.GenerateErrorResponce(result);
                }
                return  null;
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            return unitOfWork.RoleManager.Roles.Select(e => mapper.Map<RoleDTO>(e));
        }
        public void Dispose()
        {
            unitOfWork.Dispose();
        }
        public async Task<IEnumerable<PersonInRoleDTO>>  GetAllUsersInCurrentRole(string id)
        {
            var identityRole = await unitOfWork.RoleManager.FindByIdAsync(id);
            if (identityRole == null)
            {
                return null;
            }

            var resultList = new List<PersonInRoleDTO>();
            foreach(var user in  unitOfWork.UserManager.Users)

            {
               
                var person = await unitOfWork.PersonRepository.GetPersonByAccount(user.Id);
                user.Person = person;
                var userInRole = mapper.Map<PersonInRoleDTO>(user);
                //if (userInRole == null || user.ApplicationUser == null)
                //    continue;
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
            return identityRole == null ? null : mapper.Map<RoleDTO>(identityRole);
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

        public async Task<IdentityUserResult> UpdateRole(RoleDTO role)
        {
            var identityRole = await unitOfWork.RoleManager.FindByIdAsync(role.Id);
            if (identityRole == null)
            {
                return new IdentityUserResult(){NotFound = true,Errors = new List<string>(){$"Role with {role.Id}  not found"}};
            }
            identityRole.Name = role.Name;
            var result = await unitOfWork.RoleManager.UpdateAsync(identityRole);
            if (result.Succeeded)
            {
                return IdentityUserResult.SUCCESS;
            }
            else
            {
                return IdentityUserResult.GenerateErrorResponce(result);
            }

        }
    }
}
