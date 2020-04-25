using AutoMapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareholderController : ControllerBase
    {
        private readonly IShareholderService shareholderService;
        private readonly IMapper mapper;
        private readonly ILogger<ShareholderController> logger;

        public ShareholderController(IShareholderService shareholderService,IMapper mapper, ILogger<ShareholderController> logger)
        {
            this.shareholderService = shareholderService;
            this.mapper = mapper;
            this.logger = logger;
        }
        // GET /api/Shareholder
        [HttpGet]
        public async Task<IActionResult> GetAllShareholders([FromQuery]PageQueryParameters pageQueryParameters,[FromQuery]SearchShareholderQuery searchShareholderQuery)
        {
            var paginatedModel = mapper.Map<PaginatedModel<ShareholderDTO>>(pageQueryParameters);
            var filter = mapper.Map<ShareholderFilterModel>(searchShareholderQuery);
            PaginatedModel<ShareholderDTO> shareholders = null;
            if (searchShareholderQuery != null)
            {
               shareholders = await shareholderService.GetAllShareholders(pageQueryParameters.PageNumber, pageQueryParameters.PageSize,filter);
            }
            shareholders = await shareholderService.GetAllShareholders(pageQueryParameters.PageNumber, pageQueryParameters.PageSize);
            var paginatedShareholders = new PaginatedList<ShareholderDTO>(shareholders, paginatedModel );
            return Ok (paginatedShareholders);
        }
        // PUT /api/Shareholder/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShareholder(int id, ShareholderDTO shareholder)
        {
            if (id != shareholder.Id)
            {
                return BadRequest();
            }
            var currentShareholder = await shareholderService.GetShareholderById(id);
            if (currentShareholder == null)
            {
                return NotFound();
            }
            await shareholderService.UpdateShareholder(shareholder);
            return Ok(currentShareholder);

        }

        // POST: api/Shareholder
        [HttpPost]
        public async Task<IActionResult> AddShareholder(ShareholderDTO shareholder)
        {
            if (shareholder == null)
            {
                return BadRequest("Object of type shareholder is null");
            }
            else
            {
                await shareholderService.AddShareholder(shareholder);
                return Ok(shareholder);
            }
        }
        // DELETE /api/Shareholder/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShareholder(int id)
        {
            var shareholder = await shareholderService.GetShareholderById(id);
            if (shareholder == null)
            {
                return NotFound();
            }
            await shareholderService.DeleteShareholder(shareholder);
            return Ok("Deleted succesfully");

        }
    }
}