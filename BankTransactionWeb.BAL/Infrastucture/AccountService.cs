using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
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
        private readonly ILogger<AccountService> logger;
        private readonly Random random;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AccountService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            random = new Random();
        }
        public async Task AddAccount(AccountDTO account)
        {
            try
            {
                var accountMapped = mapper.Map<Account>(account);
                unitOfWork.AccountRepository.Add(accountMapped);
                await unitOfWork.Save();
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

        public async Task<IEnumerable<AccountDTO>> GetAllAccounts()
        {
            try
            {
                var accounts = (await unitOfWork.AccountRepository.GetAll());
                return accounts.Select(account => mapper.Map<AccountDTO>(account)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<AccountDTO>> GetMyAccounts(ClaimsPrincipal user)
        {
            try
            {
                var id = unitOfWork.UserManager.GetUserId(user);
                var personFinded = (await unitOfWork.PersonRepository.GetAll()).Where(e => e.ApplicationUserFkId == id).FirstOrDefault();
                var accounts = (await unitOfWork.PersonRepository.GetById(personFinded.Id)).Accounts;
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

        public string GenrateCardNumber(int numberOfDigits)
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
