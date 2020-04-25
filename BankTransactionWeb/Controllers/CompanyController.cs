using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.Models;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly ILogger<CompanyController> logger;
        private readonly IMapper mapper;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger, IMapper mapper)
        {
            this.companyService = companyService;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index(PageQueryParameters pageQueryParameters)
        {
            var companys = (await companyService.GetAllCompanies(pageQueryParameters.PageNumber, pageQueryParameters.PageSize));
            return View(companys);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies(PageQueryParameters pageQueryParameters)
        {
            var companys = (await companyService.GetAllCompanies(pageQueryParameters.PageNumber, pageQueryParameters.PageSize));
            var listOfComapniesVM = new PaginatedList<CompanyDTO>(companys);
            return View(listOfComapniesVM);
        }
   

        //public async Task<IActionResult> GetAllCompanies(string sortOrder, PageQueryParameters pageQueryParameters)
        //{
        //    ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        //    var companys = (await companyService.GetAllCompanies(pageQueryParameters.PageNumber, pageQueryParameters.PageSize));
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            companys = companys.OrderByDescending(s => s.Name);
        //            break;
        //        case "Date":
        //            companys = companys.OrderBy(s => s.DateOfCreation);
        //            break;
        //        case "date_desc":
        //            companys = companys.OrderByDescending(s => s.DateOfCreation);
        //            break;
        //        default:
        //            companys = companys.OrderBy(s => s.Name);
        //            break;
        //    }
        //    return  PartialView("AllCimpaniesGrid",companys);
        //}

        [HttpGet]
        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompany(AddCompanyViewModel companyModel)
        {
                if (companyModel == null)
                {
                    logger.LogError($"Object of type {typeof(AddCompanyViewModel)} send by client was null.");
                    return BadRequest("Object of type company is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError($"Company model send by client is not valid.");
                    return BadRequest("Company model is not valid.");
                }
                else
                {
                    var company = mapper.Map<CompanyDTO>(companyModel);
                    await companyService.AddCompany(company);
                    return RedirectToAction(nameof(GetAllCompanies));
                }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompany(int id)
        {

            try
            {
                var currentCompany = await companyService.GetCompanyById(id);
                if (currentCompany == null)
                {
                    logger.LogError($"Company with id {id} not find");
                    return NotFound();
                }
                else
                {
                    var companyModel = mapper.Map<UpdateCompanyViewModel>(currentCompany);
                    return View(companyModel);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateCompany)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompany([FromForm]UpdateCompanyViewModel companyModel)
        {
            try
            {
                if (companyModel == null)
                {
                    logger.LogError($"Object of type {typeof(UpdateCompanyViewModel)} send by client was null.");
                    return BadRequest("Object of type company is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Company model send by client is not valid.");
                    return View(companyModel);
                }
                else
                {
                    try
                    {
                        var company = await companyService.GetCompanyById(companyModel.Id);
                        if (company == null)
                        {
                            logger.LogError($"Company with id {companyModel.Id} not find");
                            return NotFound();
                        }
                        else
                        {
                            var updatedCompany = mapper.Map<UpdateCompanyViewModel, CompanyDTO>(companyModel, company);
                            await companyService.UpdateCompany(updatedCompany);
                            return RedirectToAction(nameof(GetAllCompanies));
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        logger.LogError($"Unable to update company becuase of {ex.Message}");
                        ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                        return View(companyModel);
                    }

                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateCompany)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }


        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var company = await companyService.GetCompanyById(id);
                if (company == null)
                {
                    logger.LogError($"Company with id {id} not find");
                    return NotFound();
                }
                try
                {
                    await companyService.DeleteCompany(company);
                    return RedirectToAction(nameof(GetAllCompanies));
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update person becuase of {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteCompany)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }


        protected override void Dispose(bool disposing)
        {
            companyService.Dispose();
            base.Dispose(disposing);
        }
    }
}