
using BankTransaction.BAL.Abstract;
using BankTransaction.Web.Mapper;
using BankTransaction.Web.Mapper.Filters;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Web.Mapper;

namespace BankTransaction.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShareholderController : Controller
    {
        private readonly IShareholderService shareholderService;
        private readonly ILogger<ShareholderController> logger;
        private readonly ICompanyService companyService;
        private readonly IPersonService personService;


        public ShareholderController(IShareholderService shareholderService, ILogger<ShareholderController> logger,
            ICompanyService companyService, IPersonService personService)
        {
            this.shareholderService = shareholderService;
            this.logger = logger;
            this.companyService = companyService;
            this.personService = personService;
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 30000)]
        public async Task<IActionResult> GetAllShareholders(ShareholderSearchModel searchModel = null, PageQueryParameters pageQueryParameters = null)
        {
            var filter = ShareholderSearchToFilterDto.Instance.Map(searchModel);
            var allshareholders = await shareholderService.GetAllShareholders(pageQueryParameters.PageNumber, pageQueryParameters.PageSize, filter);
            var listOfShareholdersVM = new PaginatedList<ShareholderDTO>(allshareholders);
            return View(listOfShareholdersVM);
        }

        [HttpGet]
        public async Task<IActionResult> AddShareholder(int id)
        {
            var person = await personService.GetPersonById(id);
            if (person != null)
            {
                var shareholderVM = new AddShareholderViewModel()
                {
                    //Person = person,
                    PersonSurName = person.Surname,
                    PersonName = person.Name,
                    PersonLastName = person.LastName,
                    PersonId = id,
                    Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name")
                };
                return View(shareholderVM);
            };
            return NotFound("Sorry. Current user not found");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShareholder(AddShareholderViewModel shareholderModel)
        {
            shareholderModel.Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name");
            if (ModelState.IsValid)
            {
                var shareholder = AddShareholderToShareholderDTOMapper.Instance.Map(shareholderModel);
                var result = await shareholderService.AddShareholder(shareholder);
                if (result.IsError)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(shareholderModel);
                }
                return RedirectToAction(nameof(GetAllShareholders));
            }
            return View(shareholderModel);

        }

        [HttpGet]
        public async Task<IActionResult> UpdateShareholder(int id)
        {
            var currentShareholder = await shareholderService.GetShareholderById(id);
            if (currentShareholder == null)
            {
                return NotFound($"Shareholder with id {id} not find");
            }
            else
            {
                var shareholderModel = UpdateShareholderToShareholderDTOMapper.Instance.MapBack(currentShareholder);
                shareholderModel.Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name");
               // shareholderModel.PersonId = (await personService.GetPersonById(shareholderModel.PersonId)).Id;
                return View(shareholderModel);
            }



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateShareholder([FromForm]UpdateShareholderViewModel shareholderModel)
        {
            shareholderModel.Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name");
            if (ModelState.IsValid)
            {
                try
                {
                    var shareholder = await shareholderService.GetShareholderById(shareholderModel.Id);
                    if (shareholder == null)
                    {
                        return NotFound($"Shareholder with id {shareholderModel.Id} not find");
                    }
                    else
                    {
                        var updatedShareholder = UpdateShareholderToShareholderDTOMapper.Instance.Map(shareholderModel);
                        var result = await shareholderService.UpdateShareholder(updatedShareholder);
                        if (result.IsError)
                        {
                            ModelState.AddModelError("", result.Message);
                            return View(shareholderModel);
                        }
                        return RedirectToAction(nameof(GetAllShareholders));
                    }
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
            else
            {
                return View(shareholderModel);
            }

        }


        public async Task<IActionResult> DeleteShareholder(int id)
        {

            var shareholder = await shareholderService.GetShareholderById(id);
            if (shareholder == null)
            {

                return NotFound($"Shareholder with id {id} not find");
            }
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