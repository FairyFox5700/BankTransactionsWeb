using AutoMapper;
using BankTransactionWeb.Areas.Admin.Models.ViewModels;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Areas.Admin.Çontrollers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly Logger<AdminController> logger;
        private readonly IMapper mapper;
        private readonly IAdminService adminService;

        public AdminController(Logger<AdminController> logger, IMapper mapper, IAdminService adminService)
        {
            this.logger = logger;
            this.mapper = mapper;
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
                var role = mapper.Map<RoleDTO>(model);
                var result = await adminService.AddRole(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(GetAllRoles));
                }
                AddModelErrors(result);
            }
            return View(model);
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
            var roles = adminService.GetAllRoles().Select(e=> mapper.Map<ListRoleViewModel>(e));
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            var role = await adminService.GetRoleWithUsers(id);
            if (role == null)
            {
                logger.LogError($"Role wwith id {id} was not finded");
                return NotFound();
            }
            var model = mapper.Map<UpdateRoleViewModel>(role);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            var role = await adminService.GetRoleById(model.Id);
            if (role == null)
            {
                logger.LogError($"Role wwith id {model.Id} was not finded");
                return NotFound();
            }
            var returnModel = mapper.Map<UpdateRoleViewModel>(role);
            var result = await adminService.UpdateRole(role.Id);
            if(result.Succeeded)
            {
                return RedirectToAction(nameof(GetAllRoles));
            }
            if(result==null)
            {
                return NotFound();
            }
            AddModelErrors(result);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var currentRole = adminService.GetRoleById(roleId);
            if(currentRole==null)
            {
                logger.LogError($"Role wwith id {roleId} was not finded");
                return NotFound();
            }
            var users = await adminService.GetAllUsersInCurrentRole(roleId);
            var model = users.Select(u => mapper.Map<UsersInRoleViewModel>(u));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsersInRole(List<UsersInRoleViewModel> model, string roleId)
        {
            ViewBag.roleId = roleId;
            var currentRole = await adminService.GetRoleById(roleId);
            if (currentRole == null)
            {
                logger.LogError($"Role wwith id {roleId} was not finded");
                return NotFound();
            }
            for(int i=0; i<model.Count();i++)
            {
                var result = await adminService.AddUserToRole(model[i].Id, model[i].IsSelected, currentRole.Name); 
                if(result.Succeeded)
                {
                    if (i < model.Count - 1)
                        continue;
                    else return RedirectToAction(nameof(GetAllRoles));
                }
                if (result == null)
                    continue;
            }
            // var model = mapper.Map<UsersInRoleViewModel>(await adminService.GetAllUsersInCurrentRole(roleId));
            return View(nameof(UpdateRoleViewModel), new { roleId = roleId });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}