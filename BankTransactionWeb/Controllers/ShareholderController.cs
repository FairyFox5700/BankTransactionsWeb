using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShareholderController : Controller
    {
        private readonly IShareholderService shareholderService;
        private readonly ILogger<ShareholderController> logger;
        private readonly IMapper mapper;
        private readonly ICompanyService companyService;
        private readonly IPersonService personService;


        public ShareholderController(IShareholderService shareholderService, ILogger<ShareholderController> logger, IMapper mapper,
            ICompanyService companyService, IPersonService personService)
        {
            this.shareholderService = shareholderService;
            this.logger = logger;
            this.mapper = mapper;
            this.companyService = companyService;
            this.personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShareholders(string companyName, DateTime? dateOfCompanyCreation = null)
        {
            try
            {
                var sh = await shareholderService.GetAllShareholders(companyName, dateOfCompanyCreation);
                var listOfShareholdersVM = new ShareholdersListViewModel()
                {

                    Shareholders = (sh).ToList(),
                    CompanyName = companyName,
                    DateOfCompanyCreation = dateOfCompanyCreation
                };
                logger.LogInformation("Successfully returned all shareholders");
                return View(listOfShareholdersVM);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllShareholders)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> AddShareholder()
        {
            try
            {
                var shareholderVM = new AddShareholderViewModel()
                {
                    People = new SelectList(await personService.GetAllPersons(), "Id", "Name", "Surname", "LastName"),
                    Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name")
                };

                return View(shareholderVM);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddShareholder)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShareholder(AddShareholderViewModel shareholderModel)
        {
            try
            {
                if (shareholderModel == null)
                {
                    logger.LogError($"Object of type {typeof(AddShareholderViewModel)} send by client was null.");
                    return BadRequest("Object of type shareholder is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError($"Shareholder model send by client is not valid.");
                    return BadRequest("Shareholder model is not valid.");
                }
                else
                {
                    var shareholder = mapper.Map<ShareholderDTO>(shareholderModel);
                    await shareholderService.AddShareholder(shareholder);
                    return RedirectToAction(nameof(GetAllShareholders));
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddShareholder)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateShareholder(int id)
        {
            try
            {
                var currentShareholder = await shareholderService.GetShareholderById(id);
                if (currentShareholder == null)
                {
                    logger.LogError($"Shareholder with id {id} not find");
                    return NotFound();
                }
                else
                {
                    var shareholderModel = mapper.Map<UpdateShareholderViewModel>(currentShareholder);
                    shareholderModel.People = new SelectList(await personService.GetAllPersons(), "Id", "Name", "Surname", "LastName");
                    shareholderModel.Comapnanies = new SelectList(await companyService.GetAllCompanies(), "Id", "Name");
                    return View(shareholderModel);
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateShareholder)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateShareholder([FromForm]UpdateShareholderViewModel shareholderModel)
        {
            try
            {
                if (shareholderModel == null)
                {
                    logger.LogError($"Object of type {typeof(UpdateShareholderViewModel)} send by client was null.");
                    return BadRequest("Object of type shareholder is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Shareholder model send by client is not valid.");
                    return View(shareholderModel);
                }
                else
                {
                    try
                    {
                        var shareholder = await shareholderService.GetShareholderById(shareholderModel.Id);
                        if (shareholder == null)
                        {
                            logger.LogError($"Shareholder with id {shareholderModel.Id} not find");
                            return NotFound();
                        }
                        else
                        {
                            var updatedShareholder = mapper.Map<UpdateShareholderViewModel, ShareholderDTO>(shareholderModel, shareholder);
                            await shareholderService.UpdateShareholder(updatedShareholder);
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

            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateShareholder)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }


        public async Task<IActionResult> DeleteShareholder(int id)
        {
            try
            {
                var shareholder = await shareholderService.GetShareholderById(id);
                if (shareholder == null)
                {
                    logger.LogError($"Shareholder with id {id} not find");
                    return NotFound();
                }
                await shareholderService.DeleteShareholder(shareholder);
                return RedirectToAction(nameof(GetAllShareholders));
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteShareholder)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

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