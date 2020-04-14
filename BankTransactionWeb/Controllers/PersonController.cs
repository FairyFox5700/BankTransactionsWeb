using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankTransactionWeb.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class PersonController : Controller
    {
        private readonly IPersonService personService;
        private readonly ILogger<PersonController> logger;
        private readonly IMapper mapper;

        public PersonController(IPersonService personService, ILogger<PersonController> logger, IMapper mapper)
        {
            this.personService = personService;
            this.logger = logger;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPersons(string name, string surname, string lastname,
            string accountNumber, string accountTransaction, string companyName)
        {
            try
            {
                var listOfPersonsVM = new PersonListViewModel()
                {
                    Persons = await personService.GetAllPersons(name, surname, lastname,
                    accountNumber, accountTransaction, companyName),
                    Name = name,
                    SurName = surname,
                    LastName = lastname,
                    AccoutNumber = accountNumber,
                    AccountTransactionNumber = accountTransaction,
                    CompanyName = companyName
                };
                logger.LogInformation("Successfully returned all persons");
                return View(listOfPersonsVM);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllPersons)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdatePerson(int id)
        {
            try
            {
                var currentPerson = await personService.GetPersonById(id);
                if (currentPerson == null)
                {
                    logger.LogError($"Person with id {id} not find");
                    return NotFound();
                }
                else
                {
                    var personModel = mapper.Map<UpdatePersonViewModel>(currentPerson);
                    return View(personModel);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdatePerson)}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }


        }

      


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdatePerson([FromForm]UpdatePersonViewModel personModel)
        {
            try
            {
                if (personModel == null)
                {
                    logger.LogError("Object of type person send by client was null.");
                    return BadRequest("Object of type person is null");
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        var updatedPerson = mapper.Map<PersonDTO>(personModel);
                        var result = await personService.UpdatePerson(updatedPerson);
                        if (result == null)
                        {
                            logger.LogError($"Person with id {personModel.Id} not find");
                            return NotFound();
                        }
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(GetPersonCardCabinet));
                        }

                        else
                        {
                            AddModelErrors(result);
                        }

                    }
                    catch (DbUpdateException ex)
                    {
                        logger.LogError($"Unable to update person becuase of {ex.Message}");
                        ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                        return View(personModel);
                    }

                }
                return View(personModel);

            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdatePerson)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
       // [Authorize(Roles = "User")]
       [Authorize]
        public async Task<IActionResult> GetPersonCardCabinet()
        {
            try
            {

                var currentPerson = await personService.GetPersonById(HttpContext.User);
                if (currentPerson == null)
                {
                    logger.LogError($"Person  not find");
                    return NotFound();
                }
                else
                {
                    var personModel = mapper.Map<UpdatePersonViewModel>(currentPerson);
                    return View(personModel);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetPersonCardCabinet)}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }
        private void AddModelErrors(IdentityResult result)
        {
            if (result != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var person = await personService.GetPersonById(id);
                if (person == null)
                {
                    logger.LogError($"Person with id {id} not found");
                    return NotFound();
                }
                try
                {
                    var result = await personService.DeletePerson(person);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(GetAllPersons));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to delete person becuase of {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeletePerson)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }


        protected override void Dispose(bool disposing)
        {
            personService.Dispose();
            base.Dispose(disposing);
        }
    }
}
