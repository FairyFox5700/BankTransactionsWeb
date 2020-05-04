
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.Helpers;
using BankTransaction.Web.Mapper;
using BankTransaction.Web.Models;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Controllers
{

    public class TransactionController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly IAccountService accountService;
        private readonly ILogger<TransactionController> logger;

        public TransactionController(ITransactionService transactionService, IAccountService accountService,
            ILogger<TransactionController> logger)
        {
            this.transactionService = transactionService;
            this.accountService = accountService;
            this.logger = logger;
        }

        //auth User
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyTransaction()
        {
            var transactions = (await transactionService.GetAllUserTransactions(HttpContext.User));
            return View(transactions);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTransactions(PageQueryParameters pageQueryParameters)
        {

            var transactions = (await transactionService.GetAllTransactions(pageQueryParameters.PageNumber, pageQueryParameters.PageSize));
            var transactionListVM = new PaginatedList<TransactionDTO>(transactions);
            return View(transactionListVM);


        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTransaction(int id)
        {
                var currentTransaction = await transactionService.GetTransactionById(id);
                if (currentTransaction == null)
                {
                    return NotFound($"Transaction with id {id} not find");
                }
                else
                {
                    var transactionModel = UpdateTransactionToTransactionDTOMapper.Instance.Map(currentTransaction);
                    return View(transactionModel);
                }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTransaction([FromForm]UpdateTransactionViewModel transactionModel)
        {
            if (transactionModel == null)
            {
                return BadRequest("Object of type transaction is null");
            }
            if (!ModelState.IsValid)
            {
                return View(transactionModel);
            }
            else
            {
                try
                {
                    var transaction = await transactionService.GetTransactionById(transactionModel.Id);
                    if (transaction == null)
                    {
                        return NotFound($"Transaction with id {transactionModel.Id} not find");
                    }
                    else
                    {
                        var updatedTransaction = UpdateTransactionToTransactionDTOMapper.Instance.MapBack( transactionModel);
                        var result =await transactionService.UpdateTransaction(updatedTransaction);
                        if (result.IsError)
                        {
                            ModelState.AddModelError("",result.Message);
                            return View(transactionModel);
                        }
                        return RedirectToAction("Error", "Home", result.Message);

                    }
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError($"Unable to update transaction becuase of {ex.Message}");
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                    return View(transactionModel);
                }

            }

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await transactionService.GetTransactionById(id);
            if (transaction == null)
            {
                return NotFound($"Transaction with id {id} not find");
            }
            try
            {
                await transactionService.DeleteTransaction(transaction);
                return RedirectToAction(nameof(GetAllTransactions));
            }
            catch (DbUpdateException ex)
            {
                logger.LogError($"Unable to update person becuase of {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ExecuteTransaction()
        {
            var executeTransactionVM = new ExecuteTransactionViewModel()
            {
                Accounts = new SelectList(await accountService.GetMyAccounts(HttpContext.User), "Id", "Number")
            };

            return View(executeTransactionVM);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteTransaction([FromForm]ExecuteTransactionViewModel executeTransactionViewModel)
        {
            try
            {
                executeTransactionViewModel.Accounts = new SelectList(await accountService.GetMyAccounts(HttpContext.User), "Id", "Number");
                if (ModelState.IsValid)
                {
                    var result =await transactionService.ExecuteTransaction(executeTransactionViewModel.AccountSourceId,
                        executeTransactionViewModel.AccountDestinationNumber, executeTransactionViewModel.Amount);
                    if(result.IsError)
                    {
                        ModelState.AddModelError("", result.Message);
                        return View(executeTransactionViewModel);
                    }
                    return RedirectToAction(nameof(SuccessfulTransaction),result.Message);
                }
                else
                {
                    return View(executeTransactionViewModel);
                }
            }
            catch (ValidationException vex)
            {
                ModelState.AddModelError("", vex.Message);
                return View(executeTransactionViewModel);
            }
        }

        public IActionResult SuccessfulTransaction(string message)
        {
            return View(message);
        }



        protected override void Dispose(bool disposing)
        {
            accountService.Dispose();
            transactionService.Dispose();
            base.Dispose(disposing);
        }

    }
}

//[Authorize(Roles = "Admin")]
//public async Task<IActionResult> AddTransaction()
//{
//    try
//    {

//        var transactionVM = new AddTransactionViewModel()
//        {
//            //Accounts = new SelectList(await accountService.GetAllAccounts(), "Id", "Number")
//        };

//        return View(transactionVM);
//    }
//    catch (Exception ex)
//    {
//        logger.LogError($"Catch an exception in method {nameof(AddTransaction)}. The exception is {ex.Message}. " +
//            $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
//        return StatusCode(500, "Internal server error");
//    }
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//[Authorize(Roles = "Admin")]
//public async Task<IActionResult> AddTransaction(AddTransactionViewModel transactionModel)
//{
//    try
//    {
//        if (transactionModel == null)
//        {
//            logger.LogError($"Object of type {typeof(AddTransactionViewModel)} send by client was null.");
//            return BadRequest("Object of type transaction is null");
//        }
//        if (!ModelState.IsValid)
//        {
//            logger.LogError($"Transaction model send by client is not valid.");
//            return BadRequest("Transaction model is not valid.");
//        }
//        else
//        {
//            var transaction = mapper.Map<TransactionDTO>(transactionModel);
//            await transactionService.AddTransaction(transaction);
//            return RedirectToAction(nameof(GetAllTransactions));
//        }
//    }
//    catch (Exception ex)
//    {
//        logger.LogError($"Catch an exception in method {nameof(AddTransaction)}. The exception is {ex.Message}. " +
//            $"Inner exception {ex.InnerException?.Message ?? "NONE"}");
//        return StatusCode(500, "Internal server error");
//    }
//}