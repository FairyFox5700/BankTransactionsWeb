using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BankTransactionWeb.BAL.Cofiguration;
using BankTransactionWeb.BAL.Infrastucture;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankTransactionWeb.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService transactionService;
        private readonly IAccountService accountService;
        private readonly ILogger<TransactionController> logger;
        private readonly IMapper mapper;

        public TransactionController(ITransactionService transactionService, IAccountService accountService, ILogger<TransactionController> logger, IMapper mapper)
        {
            this.transactionService = transactionService;
            this.accountService = accountService;
            this.logger = logger;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = (await transactionService.GetAllTransactions());//maybe sort them
                logger.LogInformation("Successfully returned all transactions");
                var transactionListVM = transactions.Select(tr=>mapper.Map<TransactionListViewModel>(tr)).ToList();
                return View(transactionListVM);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllTransactions)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> AddTransaction()
        {
            var transactionVM = new AddTransactionViewModel()
            {
                Accounts = new SelectList(await accountService.GetAllAccounts(),"Id","Number")
            };

            return View(transactionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTransaction(AddTransactionViewModel transactionModel)
        {
            try
            {
                if (transactionModel == null)
                {
                    logger.LogError($"Object of type {typeof(AddTransactionViewModel)} send by client was null.");
                    return BadRequest("Object of type transaction is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError($"Transaction model send by client is not valid.");
                    return BadRequest("Transaction model is not valid.");
                }
                else
                {
                    var transaction = mapper.Map<TransactionDTO>(transactionModel);
                    await transactionService.AddTransaction(transaction);
                    return RedirectToAction(nameof(GetAllTransactions));
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddTransaction)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTransaction(int id)
        {
            var currentTransaction = await transactionService.GetTransactionById(id);
            if (currentTransaction == null)
            {
                logger.LogError($"Transaction with id {id} not find");
                return NotFound();
            }
            else
            {
                var transactionModel = mapper.Map<UpdateTransactionViewModel>(currentTransaction);
                transactionModel.Accounts = new SelectList(await accountService.GetAllAccounts(), "Id", "Number");
                return View(transactionModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTransaction([FromForm]UpdateTransactionViewModel transactionModel)
        {
            try
            {
                if (transactionModel == null)
                {
                    logger.LogError($"Object of type {typeof(UpdateTransactionViewModel)} send by client was null.");
                    return BadRequest("Object of type transaction is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Transaction model send by client is not valid.");
                    return View(transactionModel);
                }
                else
                {
                    try
                    {
                        var transaction = await transactionService.GetTransactionById(transactionModel.Id);
                        if (transaction == null)
                        {
                            logger.LogError($"Transaction with id {transactionModel.Id} not find");
                            return NotFound();
                        }
                        else
                        {
                            var updatedTransaction = mapper.Map<UpdateTransactionViewModel, TransactionDTO>(transactionModel, transaction);
                            await transactionService.UpdateTransaction(updatedTransaction);
                            return RedirectToAction(nameof(GetAllTransactions));
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        logger.LogError($"Unable to update person becuase of {ex.Message}");
                        ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                        return View(transactionModel);
                    }

                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateTransaction)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }


        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                var transaction = await transactionService.GetTransactionById(id);
                if (transaction == null)
                {
                    logger.LogError($"Transaction with id {id} not find");
                    return NotFound();
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
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteTransaction)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet]
        public async Task<IActionResult> ExecuteTransaction()
        {
            var executeTransactionVM = new ExecuteTransactionViewModel()
            {
                Accounts = new SelectList(await accountService.GetAllAccounts(), "Id", "Number")
            };

            return View(executeTransactionVM);

        }

        [HttpPost]
        public async Task<IActionResult> ExecuteTransaction([FromForm]ExecuteTransactionViewModel executeTransactionViewModel)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    await transactionService.ExecuteTransaction(executeTransactionViewModel.AccountSourceId, 
                        executeTransactionViewModel.AccountDestinationNumber, executeTransactionViewModel.Amount);
                    return RedirectToAction(nameof(GetAllTransactions));
                }
                else
                {
                    return View(executeTransactionViewModel);
                }
            }
            catch(ValidationException vex)
            {
                return Content(vex.Message);
            }
        }


        protected override void Dispose(bool disposing)
        {
            transactionService.Dispose();
            base.Dispose(disposing);
        }




    }
}