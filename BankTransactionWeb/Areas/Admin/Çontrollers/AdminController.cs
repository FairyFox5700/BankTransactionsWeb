using AutoMapper;
using BankTransactionWeb.Areas.Admin.Models.ViewModels;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Admin.Controllers
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
            try
            {
                if (!ModelState.IsValid)
                {
                    logger.LogError("Update role model send by client is not valid.");
                    return View(model);
                }

                var roleToUpdate = mapper.Map<RoleDTO>(model);
                var result = await adminService.UpdateRole(roleToUpdate);
                if (result == null)
                {
                    logger.LogError($"Role with id {model.Id} was not finded");
                    return NotFound();
                }
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                }
                if (result == null)
                {
                    return NotFound();
                }
                AddModelErrors(result);
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateRole)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> UpdateUsersInRole(string roleId)
        {
            try
            {

                ViewBag.roleId = roleId;
                var users = await adminService.GetAllUsersInCurrentRole(roleId);
                if (users == null)
                {
                    logger.LogError($"Role with id {roleId} was not finded");
                    return NotFound();
                }
                var model = users.Select(u => mapper.Map<UsersInRoleViewModel>(u)).ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateUsersInRole)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }
            //return View("~/Areas/Admin/Views/Admin/UpdateUsersInRole.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsersInRole(List<UsersInRoleViewModel> model, string roleId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    logger.LogError("User in role model send by client is not valid.");
                    return View(roleId);
                }
                ViewBag.roleId = roleId;
                var currentRole = await adminService.GetRoleById(roleId);
                if (currentRole == null)
                {
                    logger.LogError($"Role with id {roleId} was not finded");
                    return NotFound();
                }
                for (int i = 0; i < model.Count(); i++)
                {
                    var result = await adminService.AddUserToRole(model[i].AppUserId, model[i].IsSelected, currentRole.Name);
                    if (result == null)
                        continue;
                    if (result.Succeeded)
                    {
                        if (i < model.Count - 1)
                            continue;
                        else return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                    }

                }
                return View(roleId);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateUsersInRole)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

            //return View("~/Areas/Admin/Views/Admin/UpdateRoleViewModel.cshtml", new { roleId = roleId });

        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult AccessDenied()
        //{
        //    return View();
        //}

        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var result = await adminService.DeleteRole(id);
                if (result == null)
                {
                    logger.LogError($"Role with Id = {id} cannot be found");
                    return NotFound();
                }
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
                }
                AddModelErrors(result);
                return RedirectToAction(nameof(GetAllRoles), "Admin", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteRole)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }

        protected override void Dispose(bool disposing)
        {
            adminService.Dispose();
            base.Dispose(disposing);
        }
    }
}