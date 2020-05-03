using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract;
using BankTransaction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BankTransaction.Api.Helpers;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly ILogger<PersonController> logger;
        private readonly IMapper mapper;

        public PersonController(IPersonService personService, ILogger<PersonController> logger, IMapper mapper )
        {
            this.personService = personService;
            this.logger = logger;
            this.mapper = mapper;
        }

        // GET /api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetAllPersons([FromQuery]PageQueryParameters pageQueryParameters, [FromQuery]SearchPersonQuery personQuery  )
        {
           
           var paginatedModel = mapper.Map<PaginatedModel<PersonDTO>>(pageQueryParameters);

            var filter = mapper.Map<PersonFilterModel>(personQuery);
            PaginatedModel<PersonDTO> allPersons = null;
            if(filter!=null)
            {
                allPersons = await personService.GetAllPersons(pageQueryParameters.PageNumber, pageQueryParameters.PageSize, filter);
            }
            else
                allPersons = await personService.GetAllPersons(pageQueryParameters.PageNumber, pageQueryParameters.PageSize);
            var persons =new PaginatedList<PersonDTO>(allPersons, paginatedModel);
            return Ok(persons);
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
                return NotFound();
            }
            await personService.DeletePerson(person);
            return Ok("Deleted successfully");
        }

    }

}
