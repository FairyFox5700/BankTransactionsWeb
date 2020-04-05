using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RestWebBankTransactionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly ICompanyService companyService;
        private readonly ILogger<CompanyController> logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            this.companyService = companyService;
            this.logger = logger;
        }
        // GET /api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetAllCompanys()
        {
            try
            {
                var companys = (await companyService.GetAllCompanies()).ToList();
                logger.LogInformation("Successfully returned all companys");
                return companys;
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllCompanys)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }
        // PUT /api/Company/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyDTO company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }
            var currentCompany = await companyService.GetCompanyById(id);
            if (currentCompany == null)
            {
                logger.LogError($"Company with id {id} not find");
                return NotFound();
            }
            try
            {
                await companyService.UpdateCompany(company);
                return Ok(currentCompany);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to update company becuase of {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                return NotFound();
            }

        }

        // POST: api/ACompany
        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDTO company)
        {
            try
            {
                if (company == null)
                {
                    logger.LogError("Object of type company send by client was null.");
                    return BadRequest("Object of type company is null");
                }
                else
                {
                    await companyService.AddCompany(company);
                    return Ok(company);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddCompany)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }
        // DELETE /api/Company/{id}
        [HttpDelete("{id}")]
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
                    return Ok("Deleted succesfully");
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update company becuase of {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteCompany)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}