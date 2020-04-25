using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly Random random;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            random = new Random();
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

        public void Dispose()
        {
            unitOfWork.Dispose();
        }


        public async Task<AccountDTO> GetAccountById(int id)
        {
            var accountFinded = await unitOfWork.AccountRepository.GetById(id);
            return mapper.Map<AccountDTO>(accountFinded);
        }

        public async Task<PaginatedModel<AccountDTO>> GetAllAccounts(int pageNumber, int pageSize)
        {

                var accounts = (await unitOfWork.AccountRepository.GetAll(pageNumber, pageSize));
            return new PaginatedModel<AccountDTO>(accounts.Select(account => mapper.Map<AccountDTO>(account)), accounts.PageNumber, accounts.PageSize, accounts.TotalCount, accounts.TotalPages);
        }

        public async Task<IEnumerable<AccountDTO>> GetMyAccounts(ClaimsPrincipal user)
        {
            
                var id = unitOfWork.UserManager.GetUserId(user);
                var personFinded = await unitOfWork.PersonRepository.GetPersonByAccount(id);
                var accounts = (await unitOfWork.PersonRepository.GetById(personFinded.Id)).Accounts;
                return accounts.Select(account => mapper.Map<AccountDTO>(account)).ToList();
        }

        public async Task UpdateAccount(AccountDTO account)
        {
                var accountMapped = mapper.Map<Account>(account);
                unitOfWork.AccountRepository.Update(accountMapped);
                await unitOfWork.Save();
        }

        public string GenerateCardNumber(int numberOfDigits)
        {
            StringBuilder genCardNumber = new StringBuilder();
            for (int i = 0; i < numberOfDigits; i++)
            {
                genCardNumber.Append(random.Next(0, 9).ToString());
            }
            return genCardNumber.ToString();
        }

    }
}
