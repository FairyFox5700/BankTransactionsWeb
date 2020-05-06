using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompanyController : BaseApiController
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
        public async Task<ApiResponse<IEnumerable<CompanyDTO>>> GetAllCompanys([FromQuery]PageQueryParameters pageQueryParameters)
        {
            var companys = (await companyService.GetAllCompanies(pageQueryParameters.PageNumber, pageQueryParameters.PageSize)).ToList();
            return new ApiResponse<IEnumerable<CompanyDTO>>(companys);
        }

        // PUT /api/Company/{id}
        [HttpPut("{id}")]
        public async Task<ApiResponse<int>> UpdateCompany(int id, CompanyDTO company)
        {
            if (id != company.Id)
            {
                return ApiResponse<int>.BadRequest;
            }
            var currentCompany = await companyService.GetCompanyById(id);
            if (currentCompany == null)
            {
                return ApiResponse<int>.NotFound;
            }
            await companyService.UpdateCompany(company);
            return new ApiResponse<int>(id);
        }

        // POST: api/ACompany
        [HttpPost]
        public async Task<ApiResponse<CompanyDTO>> AddCompany(CompanyDTO company)
        {
            await companyService.AddCompany(company);
            return new ApiResponse<CompanyDTO>(company);
            //ASK ?????CreatedAtAction(nameof(GetAllCompanys)

        }
        // DELETE /api/Company/{id}
        [HttpDelete("{id}")]
        public async Task<ApiResponse<CompanyDTO>> DeleteCompany(int id)
        {
            var company = await companyService.GetCompanyById(id);
            if (company == null)
            {
                return ApiResponse<CompanyDTO>.NotFound;
            }
            await companyService.DeleteCompany(company);
            return new ApiResponse<CompanyDTO>(company);

        }
    }
}