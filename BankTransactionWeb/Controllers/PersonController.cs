using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BankTransactionWeb.Controllers
{
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
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }


        //public IActionResult AddPerson()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddPerson(AddPersonViewModel personModel)
        //{
        //    try
        //    {
        //        if (personModel == null)
        //        {
        //            logger.LogError("Object of type person send by client was null.");
        //            return BadRequest("Object of type person is null");
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            logger.LogError("Person model send by client is not valid.");
        //            return BadRequest("Person model is not valid.");
        //        }
        //        else
        //        {
        //            var person = mapper.Map<PersonDTO>(personModel);
        //            await personService.AddPerson(person);
        //            return RedirectToAction(nameof(GetAllPersons));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Catch an exception in method {nameof(AddPerson)}. The exception is {ex.Message}. " +
        //            $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> UpdatePerson(int id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
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
                        var person = await personService.GetPersonById(personModel.Id);
                        if (person == null)
                        {
                            logger.LogError($"Person with id {personModel.Id} not find");
                            return NotFound();
                        }
                        else
                        {
                            var updatedPerson = mapper.Map<UpdatePersonViewModel, PersonDTO>(personModel, person);
                            var result = await personService.UpdatePerson(updatedPerson);
                            if (result.Succeeded)
                            {
                                return RedirectToAction(nameof(GetAllPersons));
                            }
                            else
                            {
                                AddModelErrors(result);
                            }

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

        [HttpPost]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var person = await personService.GetPersonById(id);
                if (person == null)
                {
                    logger.LogError($"Person with id {id} not find");
                    return NotFound();
                }
                try
                {
                    var result = await personService.DeletePerson(person);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(GetAllPersons));
                    }
                    else
                    {
                        AddModelErrors(result);
                        return NotFound();
                    }
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update person becuase of {ex.Message}");
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
//[HttpGet("{id}")]
//public async Task<IActionResult> GetPersonById(int id = 1)
//{
//    try
//    {
//        var person = await personService.GetPersonById(id);//maybe sort them
//        if (person == null)
//        {
//            logger.LogInformation($"Person with id {id} not find");
//            return NotFound();
//        }
//        else
//        {
//            logger.LogInformation($"Successfully returned person with id: {id}");
//            return Ok(person);
//        }

//    }
//    catch (Exception ex)
//    {
//        logger.LogError($"Catch an exception in method {nameof(GetPersonById)}. The exception is {ex.Message}. " +
//            $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
//        return StatusCode(500, "Internal server error");
//    }
//}