using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RestWebBankTransactionApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly ILogger<PersonController> logger;

        public PersonController(IPersonService personService, ILogger<PersonController> logger)
        {
            this.personService = personService;
            this.logger = logger;
        }
        // GET /api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAllPersons()
        {
            try
            {
                var persons = await personService.GetAllPersons();
                logger.LogInformation("Successfully returned all persons");
                return persons;
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllPersons)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }
        // PUT /api/Person/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonDTO person)
        {
            if(id!=person.Id)
            {
                return BadRequest();
            }
            var currentPerson = await personService.GetPersonById(id);
            if (currentPerson == null)
            {
                logger.LogError($"Person with id {id} not find");
                return NotFound();
            }
            try
            {
                    await personService.UpdatePerson(person);
                return Ok(currentPerson);
            }
            catch (Exception ex) 
            {
                logger.LogError($"Unable to update person becuase of {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                return NotFound();
            }

        }

        // POST: api/APerson
        [HttpPost]
        public async Task<IActionResult> AddPerson(  PersonDTO person)
        {
            try
            {
                if (person == null)
                {
                    logger.LogError("Object of type person send by client was null.");
                    return BadRequest("Object of type person is null");
                }
                else
                {
                    await personService.AddPerson(person);
                    return Ok(person);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddPerson)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }
        // DELETE /api/Person/{id}
        [HttpDelete("{id}")]
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
                    await personService.DeletePerson(person);
                    return Ok("Deleted succesfully");
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
        
    }
}