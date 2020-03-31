using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
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

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task AddAccount(AccountDTO account)
        {
            var accountMapped = mapper.Map<Account>(account);
            unitOfWork.AccountRepository.Add(accountMapped);
            await unitOfWork.Save();
        }

        public async Task DeleteAccount(AccountDTO account)
        {
            var accountMapped = mapper.Map<Account>(account);
            unitOfWork.AccountRepository.Delete(accountMapped);
            await unitOfWork.Save();
        }

        public async Task<AccountDTO> GetAccountById(int id)
        {
            var accountFinded = await unitOfWork.AccountRepository.GetAll();
            return mapper.Map<AccountDTO>(accountFinded);
        }

        public async  Task<IEnumerable<AccountDTO>> GetAllAccounts()
        {
            var accounts= await unitOfWork.AccountRepository.GetAll();
            return accounts.Select(account => mapper.Map<AccountDTO>(accounts));
        }

        public async Task UpdateAccount(AccountDTO account)
        {
            var accountMapped = mapper.Map<Account>(account);
            unitOfWork.AccountRepository.Update(accountMapped);
            await unitOfWork.Save();
        }
    }
}
