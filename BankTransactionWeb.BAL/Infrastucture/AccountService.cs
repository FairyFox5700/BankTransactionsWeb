using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<AccountService> logger;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AccountService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task AddAccount(AccountDTO account)
        {
            try
            {
                var accountMapped = mapper.Map<Account>(account);
                unitOfWork.AccountRepository.Add(accountMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddAccount)} instance of account successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddAccount)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task DeleteAccount(AccountDTO account)
        {
            var accountMapped = mapper.Map<Account>(account);
            unitOfWork.AccountRepository.Delete(accountMapped);
            await unitOfWork.Save();
            try
            {
                logger.LogInformation($"In method {nameof(DeleteAccount)} instance of account successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteAccount)} in class {nameof(PersonService)}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public async Task<AccountDTO> GetAccountById(int id)
        {
                try
                {
                var accountFinded = await unitOfWork.AccountRepository.GetById(id);
            return mapper.Map<AccountDTO>(accountFinded);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAccountById)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async  Task<IEnumerable<AccountDTO>> GetAllAccounts()
        {
            try
            {
                var accounts= await unitOfWork.AccountRepository.GetAll();
            return accounts.Select(account => mapper.Map<AccountDTO>(account));
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllAccounts)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task UpdateAccount(AccountDTO account)
        {
            try
            {
                var accountMapped = mapper.Map<Account>(account);
            unitOfWork.AccountRepository.Update(accountMapped);
            await unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateAccount)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }
       

      
    }
}
