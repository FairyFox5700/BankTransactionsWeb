using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            try
            {
                var shareholders = (await shareholderService.GetAllShareholders()).ToList(); 
                logger.LogInformation("Successfully returned all shareholders");
                return shareholders;
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllShareholders)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
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
            try
            {
                await shareholderService.UpdateShareholder(shareholder);
                return Ok(currentShareholder);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to update shareholder becuase of {ex.Message}");
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists, " +
                "see your system administrator.");
                return NotFound();
            }

        }

        // POST: api/AShareholder
        [HttpPost]
        public async Task<IActionResult> AddShareholder(ShareholderDTO shareholder)
        {
            try
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
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddShareholder)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }
        // DELETE /api/Shareholder/{id}
        [HttpDelete("{id}")]
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
                try
                {
                    await shareholderService.DeleteShareholder(shareholder);
                    return Ok("Deleted succesfully");
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update shareholder becuase of {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteShareholder)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}