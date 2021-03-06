﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.BAL.Abstract;
using BankTransaction.Models.Validation;
using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Web.Mapper.Admin;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankTransaction.Web.Areas.Admin.Çontrollers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> logger;
        private readonly IAdminService adminService;

        public AdminController(ILogger<AdminController> logger,  IAdminService adminService)
        {
            this.logger = logger;
            this.adminService = adminService;
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            
                if (ModelState.IsValid)
                {
                    var role = RoleDTOToAddRoleMapper.Instance.MapBack(model);
                    var result = await adminService.AddRole(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                    }
                    AddModelErrors(result);
                }
                return View(model);
        }


      
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = adminService.GetAllRoles().ToList();//Select(e => RoleDTOToListRoleMapper.Instance.Map(e)).REMOVE
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            var role = await adminService.GetRoleWithUsers(id);
            if (role == null)
            {
                return NotFound();
            }
            var model = RoleDTOToUpdateRoleMapper.Instance.Map(role);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var roleToUpdate = RoleDTOToUpdateRoleMapper.Instance.MapBack(model);
            var result = await adminService.UpdateRole(roleToUpdate);
            if (result.NotFound)
            {
                return NotFound(result);
            }
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
            }
            AddModelErrors(result);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var users = await adminService.GetAllUsersInCurrentRole(roleId);
            if (users == null)
            {
                //smth here
                return NotFound("Current role was not found");
            }
            var model = users.Select(u => PersonInRoleDTOToUserRolesMapper.Instance.Map(u)).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsersInRole(List<UsersInRoleViewModel> model, string roleId)
        {
            ViewBag.roleId = roleId;
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //    //return RedirectToAction(nameof(UpdateUsersInRole)), "Admin", new { roleId = roleId }); 
            //}
            var currentRole = await adminService.GetRoleById(roleId);
            if (currentRole == null)
            {
                return NotFound("Current role was not found");
            }
            for (var i = 0; i < model.Count(); i++)
            {
                var result = await adminService.AddUserToRole(model[i].AppUserId, model[i].IsSelected, currentRole.Name);
                if (result == null)
                    continue;
                if (result.NotFound)
                    return NotFound((result.Errors));
                if (!result.Succeeded)
                {
                    return RedirectToAction("Error", "Home",new ErrorViewModel( result.Errors.ToList()));
                }
                if(result.Succeeded)
                {
                    if (i < model.Count - 1)
                        continue;
                    else
                    {
                        return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                    }
                }
               
            }
            return RedirectToAction(nameof(GetAllRoles));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
                var result = await adminService.DeleteRole(id);
                if (result.NotFound)
                {
                    return NotFound(result);
                }
                else if (result.Succeeded)
                {
                    return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                }
                AddModelErrors(result);
                return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
        }
        private void AddModelErrors(IdentityUserResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            adminService.Dispose();
            base.Dispose(disposing);
        }
    }
}