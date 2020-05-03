using System.Threading.Tasks;
using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.Models;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankTransaction.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShareholderController : Controller
    {
        private readonly ICompanyService companyService;
        private readonly ILogger<ShareholderController> logger;
        private readonly IMapper mapper;
        private readonly IPersonService personService;
        private readonly IShareholderService shareholderService;


        public ShareholderController(IShareholderService shareholderService, ILogger<ShareholderController> logger,
            IMapper mapper,
            ICompanyService companyService, IPersonService personService)
        {
            this.shareholderService = shareholderService;
            this.logger = logger;
            this.mapper = mapper;
            this.companyService = companyService;
            this.personService = personService;
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 30000)]
        public async Task<IActionResult> GetAllShareholders(ShareholderSearchModel searchModel = null,
            PageQueryParameters pageQueryParameters = null)
        {
            var filter = mapper.Map<ShareholderFilterModel>(searchModel);
            var allshareholders = await shareholderService.GetAllShareholders(pageQueryParameters.PageNumber,
                pageQueryParameters.PageSize, filter);
            var listOfShareholdersVM = new PaginatedList<ShareholderDTO>(allshareholders);
            return View(listOfShareholdersVM);
        }

        public async Task<IActionResult> AddShareholder(int id)
        {
            var person = await personService.GetPersonById(id);
            if (person != null)
            {
                var shareholderVM = new AddShareholderViewModel
                {
                    Person = person,
                    PersonId = id,
                    Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name")
                };
                return View(shareholderVM);
            }

            ;
            return NotFound("Sorry. Current user not found");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShareholder(AddShareholderViewModel shareholderModel)
        {
            if (!ModelState.IsValid) return View(shareholderModel);

            var shareholder = mapper.Map<ShareholderDTO>(shareholderModel);
            await shareholderService.AddShareholder(shareholder);
            return RedirectToAction(nameof(GetAllShareholders));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateShareholder(int id)
        {
            var currentShareholder = await shareholderService.GetShareholderById(id);
            if (currentShareholder == null) return NotFound($"Shareholder with id {id} not find");

            var shareholderModel = mapper.Map<UpdateShareholderViewModel>(currentShareholder);
            shareholderModel.Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name");
            shareholderModel.Person = await personService.GetPersonById(id);
            return View(shareholderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateShareholder([FromForm] UpdateShareholderViewModel shareholderModel)
        {
            if (shareholderModel == null) return BadRequest("Object of type shareholder is null");
            if (!ModelState.IsValid)
                return View(shareholderModel);
            try
            {
                var shareholder = await shareholderService.GetShareholderById(shareholderModel.Id);
                if (shareholder == null) return NotFound($"Shareholder with id {shareholderModel.Id} not find");

                var updatedShareholder = mapper.Map(shareholderModel, shareholder);
                await shareholderService.UpdateShareholder(updatedShareholder);
                return RedirectToAction(nameof(GetAllShareholders));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"Unable to update shareholder because of {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists, " +
                                             "see your system administrator.");
                return View(shareholderModel);
            }
        }


        public async Task<IActionResult> DeleteShareholder(int id)
        {
            var shareholder = await shareholderService.GetShareholderById(id);
            if (shareholder == null) return NotFound($"Shareholder with id {id} not find");
            await shareholderService.DeleteShareholder(shareholder);
            return RedirectToAction(nameof(GetAllShareholders));
        }


        protected override void Dispose(bool disposing)
        {
            companyService.Dispose();
            personService.Dispose();
            shareholderService.Dispose();
            base.Dispose(disposing);
        }
    }
}