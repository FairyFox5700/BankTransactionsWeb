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
                throw ex;

            }
        }

        public async Task DeleteAccount(AccountDTO account)
        {

            try
            {
                var accountMapped = mapper.Map<Account>(account);
                unitOfWork.AccountRepository.Delete(accountMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(DeleteAccount)} instance of account successfully added");
            }
            catch (Exception ex)
            {
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
                throw ex;

            }
        }

        public async  Task<IEnumerable<AccountDTO>> GetAllAccounts()
        {
            try
            {
                var accounts= (await unitOfWork.AccountRepository.GetAll());
            return accounts.Select(account => mapper.Map<AccountDTO>(account)).ToList();
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        public async Task<IEnumerable<AccountDTO>> GetMyAccounts(int userId)
        {
            try
            {
                var accounts = (await unitOfWork.PersonRepository.GetById(userId)).Accounts;
                return accounts.Select(account => mapper.Map<AccountDTO>(account)).ToList();
            }
            catch (Exception ex)
            {
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
                throw ex;

            }
        }

    



    }
}
