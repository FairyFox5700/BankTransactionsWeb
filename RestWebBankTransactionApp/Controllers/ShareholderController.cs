using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebBankTransactionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareholderController : ControllerBase
    {
        private readonly IShareholderService shareholderService;
        private readonly ILogger<ShareholderController> logger;

        public ShareholderController(IShareholderService shareholderService, ILogger<ShareholderController> logger)
        {
            this.shareholderService = shareholderService;
            this.logger = logger;
        }
        // GET /api/Shareholder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShareholderDTO>>> GetAllShareholders()
        {
            var shareholders = (await shareholderService.GetAllShareholders()).ToList();
            logger.LogInformation("Successfully returned all shareholders");
            return shareholders;
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
                logger.LogError($"Shareholder with id {id} not find");
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
                logger.LogError("Object of type shareholder send by client was null.");
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
                logger.LogError($"Shareholder with id {id} not find");
                return NotFound();
            }
            await shareholderService.DeleteShareholder(shareholder);
            return Ok("Deleted succesfully");

        }
    }
}