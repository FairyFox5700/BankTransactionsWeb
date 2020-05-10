using BankTransaction.BAL.Abstract;
using BankTransaction.Web.Helpers;
using BankTransaction.Web.Mapper;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.Mapper.OldMapper;

namespace BankTransaction.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly ILogger<CompanyController> logger;

        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            this.companyService = companyService;
            this.logger = logger;
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
                    return BadRequest("Object of type company is null");
                }
                if (ModelState.IsValid)
                {
                    var company = AddCompanyToCompanyDTOMapper.Instance.Map(companyModel);
                    await companyService.AddCompany(company);
                    return RedirectToAction(nameof(GetAllCompanies));
                }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCompany(int id)
        {
            var currentCompany = await companyService.GetCompanyById(id);
            if (currentCompany == null)
            {
                return NotFound($"Company with id {id} not find");
            }
            else
            {
                var companyModel = UpdateCompanyToCompanyDTOMapper.Instance.MapBack(currentCompany);
                return View(companyModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCompany([FromForm]UpdateCompanyViewModel companyModel)
        {
            
                if (companyModel == null)
                {                    
                    return BadRequest("Object of type company is null");
                }
                if (!ModelState.IsValid)
                {
                    return View(companyModel);
                }
                else
                {
                    try
                    {
                        var company = await companyService.GetCompanyById(companyModel.Id);
                        if (company == null)
                        {
                            return NotFound($"Company with id {companyModel.Id} not find");
                        }
                        else
                        {
                            var updatedCompany = UpdateCompanyToCompanyDTOMapper.Instance.Map(companyModel);
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


        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await companyService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound($"Company with id {id} not find");
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


        protected override void Dispose(bool disposing)
        {
            companyService.Dispose();
            base.Dispose(disposing);
        }
    }
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