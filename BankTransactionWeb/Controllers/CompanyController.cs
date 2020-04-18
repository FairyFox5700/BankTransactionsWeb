using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companys = (await companyService.GetAllCompanies());//maybe sort them
                logger.LogInformation("Successfully returned all companies");
                return View(companys);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllCompanies)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCompany(AddCompanyViewModel companyModel)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddCompany)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
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