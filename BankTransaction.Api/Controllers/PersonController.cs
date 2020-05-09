using AutoMapper;
using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Mapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : BaseApiController
    {
        private readonly IPersonService personService;
        private readonly ILogger<PersonController> logger;

        public PersonController(IPersonService personService, ILogger<PersonController> logger, IMapper mapper)
        {
            this.personService = personService;
            this.logger = logger;
        }

        // GET /api/Person
        [HttpGet]
        public async Task<ApiDataResponse<PaginatedList<PersonDTO>>> GetAllPersons([FromQuery]PageQueryParameters pageQueryParameters = null, [FromQuery]SearchPersonQuery personQuery = null)
        {

            var paginatedModel = PaginatedModelPersonToQueryList.Instance.MapBack(pageQueryParameters);

            var filter = PersonFilterToSearchMapper.Instance.MapBack(personQuery);
            var allPersons = await personService.GetAllPersons(pageQueryParameters.PageNumber, pageQueryParameters.PageSize, filter);
            var persons = new PaginatedList<PersonDTO>(allPersons, paginatedModel);
            return new ApiDataResponse<PaginatedList<PersonDTO>>(persons);
        }
        // PUT /api/Person/{id}
        [HttpPut("{id}")]
        public async Task<ApiDataResponse<int>> UpdatePerson(int id, PersonDTO person)
        {
            if (id != person.Id)
            {
                return ApiDataResponse<int>.BadRequest;
            }
            var currentPerson = await personService.GetPersonById(id);
            if (currentPerson == null)
            {
                return ApiDataResponse<int>.NotFound;
            }
            await personService.UpdatePerson(person);
            return new ApiDataResponse<int>(id);

        }

        // POST: api/APerson
        [HttpPost]
        public async Task<ApiDataResponse<PersonDTO>> AddPerson(PersonDTO person)
        {
            await personService.AddPerson(person);
            return new ApiDataResponse<PersonDTO>(person);
        }
        // DELETE /api/Person/{id}
        [HttpDelete("{id}")]
        public async Task<ApiDataResponse<PersonDTO>> DeletePerson(int id)
        {
            var person = await personService.GetPersonById(id);
            if (person == null)
            {
                return ApiDataResponse<PersonDTO>.NotFound;
            }
            await personService.DeletePerson(person);
            return new ApiDataResponse<PersonDTO>(person);
        }

    }

}
