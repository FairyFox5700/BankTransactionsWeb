using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.Controllers
{
    public class UserController:Controller
    {
        private readonly ITransactionService transactionService;
        private readonly IAccountService accountService;
        private readonly ILogger<UserController> logger;
        private readonly IMapper mapper;

        public UserController(ITransactionService transactionService, IAccountService accountService, ILogger<UserController> logger, IMapper mapper)
        {
            this.transactionService = transactionService;
            this.accountService = accountService;
            this.logger = logger;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MyTransaction(int userId)
        {
            try
            {
                var transactions = (await transactionService.GetAllUserTransactions(userId));//maybe sort them
                logger.LogInformation("Successfully returned all  user transactions");
                var transactionListVM = transactions.Select(tr => mapper.Map<TransactionListViewModel>(tr)).ToList();
                return View(transactionListVM);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(MyTransaction)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }

        //auth User
        [HttpGet]
        public async Task<IActionResult> MyAccounts(int userId)
        {
            try
            {
                var accounts = (await accountService.GetMyAccounts(userId)).ToList();//maybe sort them
                logger.LogInformation("Successfully returned all accounts");
                return View(accounts);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(MyAccounts)}. The exception is {ex.Message}. " +
                    $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
