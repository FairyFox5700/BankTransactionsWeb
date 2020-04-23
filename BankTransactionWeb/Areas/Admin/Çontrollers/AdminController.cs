using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Models.Validation;

namespace BankTransaction.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> logger;
        private readonly IMapper mapper;
        private readonly IAdminService adminService;

        public AdminController(ILogger<AdminController> logger, IMapper mapper, IAdminService adminService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.adminService = adminService;
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
            // return View("~/Areas/Admin/Views/Admin/AddRole.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var role = mapper.Map<RoleDTO>(model);
                    var result = await adminService.AddRole(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                    }
                    AddModelErrors(result);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddRole)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

            //return View("~/Areas/Admin/Views/Admin/AddRole.cshtml", model);
        }

        private void AddModelErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private void AddModelErrors(IdentityUserResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
      
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            try
            {
                var roles = adminService.GetAllRoles().Select(e => mapper.Map<ListRoleViewModel>(e)).ToList();
                return View(roles);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllRoles)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }
            //return View("~/Areas/Admin/Views/Admin/GetAllRoles.cshtml", roles);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            try
            {

                var role = await adminService.GetRoleWithUsers(id);
                if (role == null)
                {
                    logger.LogError($"Role wwith id {id} was not finded");
                    return NotFound();
                }
                var model = mapper.Map<UpdateRoleViewModel>(role);
                return View(model);
                //return View("~/Areas/Admin/Views/Admin/UpdateRole.cshtml", model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateRole)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var roleToUpdate = mapper.Map<RoleDTO>(model);
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
                return NotFound();
            }
            var model = users.Select(u => mapper.Map<UsersInRoleViewModel>(u)).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsersInRole(List<UsersInRoleViewModel> model, string roleId)
        {
            ViewBag.roleId = roleId;
            if (!ModelState.IsValid)
            {
                return View(roleId);
            }
            var currentRole = await adminService.GetRoleById(roleId);
            if (currentRole == null)
            {
                //smth here
                return NotFound();
            }
            for (var i = 0; i < model.Count(); i++)
            {
                var result = await adminService.AddUserToRole(model[i].AppUserId, model[i].IsSelected, currentRole.Name);
                if (result.NotFound)
                    return NotFound((result));
                if (!result.Succeeded) continue;
                if (i < model.Count - 1)
                    continue;
                else
                {
                    AddModelErrors(result);
                    return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                }
            }
            return RedirectToAction(nameof(GetAllRoles));
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult AccessDenied()
        //{
        //    return View();
        //}

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

        protected override void Dispose(bool disposing)
        {
            adminService.Dispose();
            base.Dispose(disposing);
        }
    }
}