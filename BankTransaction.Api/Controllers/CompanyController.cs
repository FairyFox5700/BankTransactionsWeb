using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<CompanyDTO>>> GetAllCompanys()
        {
            var companys = (await companyService.GetAllCompanies()).ToList();
            return companys;
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
            await companyService.UpdateCompany(company);
            return Ok(currentCompany);
        }

        // POST: api/ACompany
        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDTO company)
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
        // DELETE /api/Company/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await companyService.GetCompanyById(id);
            if (company == null)
            {
                logger.LogError($"Company with id {id} not find");
                return NotFound();
            }
            await companyService.DeleteCompany(company);
            return Ok("Deleted succesfully");

        }
    }
}