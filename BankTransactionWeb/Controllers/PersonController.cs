using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Models;
using BankTransaction.Web.Helpers;
using BankTransaction.Web.Models;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 30000)]
        public async Task<IActionResult> GetAllPersons([FromQuery]PersonSearchModel personSearch = null, PageQueryParameters pageQueryParameters = null)
        {

            var filter = mapper.Map<PersonFilterModel>(personSearch);
            var allPersons = await personService.GetAllPersons(pageQueryParameters.PageNumber, pageQueryParameters.PageSize, filter);
            var listOfPersonsVM = new PaginatedList<PersonDTO>(allPersons);
            return View(listOfPersonsVM);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdatePerson(int id)
        {

            var currentPerson = await personService.GetPersonById(id);
            if (currentPerson == null)
            {
                return NotFound($"Person with id {id} not find");
            }
            else
            {
                var personModel = mapper.Map<UpdatePersonViewModel>(currentPerson);
                return View(personModel);
            }

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdatePerson([FromForm]UpdatePersonViewModel personModel)
        {
            if (personModel == null)
            {
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
                    else if (result.Succeeded)
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


        public async Task<IActionResult> PersonSearch([FromQuery]PersonSearchModel personSearch)
        {
            var filter = mapper.Map<PersonFilterModel>(personSearch);
            var allPersons = await personService.GetAllPersons(1, 30, filter);
            return Json(allPersons);
        }
        [HttpGet]
        // [Authorize(Roles = "User")]
        [Authorize]
        public async Task<IActionResult> GetPersonCardCabinet(int id = 0)
        {

            var currentPerson = id == 0 ? await personService.GetPersonById(HttpContext.User) : await personService.GetPersonById(id);

            if (currentPerson == null)
            {
                return NotFound($"Person  not find");
            }
            else
            {
                var personModel = mapper.Map<UpdatePersonViewModel>(currentPerson);
                return View(personModel);
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
                else if (result.Succeeded)
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


        protected override void Dispose(bool disposing)
        {
            personService.Dispose();
            base.Dispose(disposing);
        }
    }
}
