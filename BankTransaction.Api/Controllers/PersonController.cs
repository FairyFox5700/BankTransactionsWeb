using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
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
            var persons = await personService.GetAllPersons();
            logger.LogInformation("Successfully returned all persons");
            return persons;
        }
        // PUT /api/Person/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonDTO person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            var currentPerson = await personService.GetPersonById(id);
            if (currentPerson == null)
            {
                logger.LogError($"Person with id {id} not find");
                return NotFound();
            }
            await personService.UpdatePerson(person);
            return Ok(currentPerson);

        }

        // POST: api/APerson
        [HttpPost]
        public async Task<IActionResult> AddPerson(PersonDTO person)
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
        // DELETE /api/Person/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await personService.GetPersonById(id);
            if (person == null)
            {
                logger.LogError($"Person with id {id} not find");
                return NotFound();
            }
            await personService.DeletePerson(person);
            return Ok("Deleted succesfully");
        }

    }

}
