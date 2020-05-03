using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Controllers
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
        [Cached(200)]
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetAllCompanys([FromQuery]PageQueryParameters pageQueryParameters)
        {
            var companys = (await companyService.GetAllCompanies(pageQueryParameters.PageNumber, pageQueryParameters.PageSize)).ToList();
            return Ok( companys);
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
                return NotFound();
            }
            await companyService.UpdateCompany(company);
            return Ok(currentCompany);
        }

        // POST: api/ACompany
        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDTO company)
        {
            if (company == null)
            {
                return BadRequest("Object of type company is null");
            }
            else
            {
                await companyService.AddCompany(company);
                return Ok(company);
            }
        }
        // DELETE /api/Company/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await companyService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }
            await companyService.DeleteCompany(company);
            return Ok("Deleted succesfully");

        }
    }
}