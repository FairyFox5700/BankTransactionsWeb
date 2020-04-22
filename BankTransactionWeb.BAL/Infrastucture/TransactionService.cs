using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using BankTransaction.DAL.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BankTransaction.Models.Validation;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<TransactionService> logger;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TransactionService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task AddTransaction(TransactionDTO transaction)
        {
            try
            {
                var transactionMapped = mapper.Map<Transaction>(transaction);
                transactionMapped.DateOftransfering = DateTime.Now;
                unitOfWork.TransactionRepository.Add(transactionMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(AddTransaction)} instance of transaction was successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(AddTransaction)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task DeleteTransaction(TransactionDTO transaction)
        {
            try
            {
                var transactionMapped = mapper.Map<Transaction>(transaction);
                unitOfWork.TransactionRepository.Delete(transactionMapped);
                await unitOfWork.Save();
                logger.LogInformation($"In method {nameof(DeleteTransaction)} instance of transaction successfully added");
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(DeleteTransaction)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllTransactions()
        {
            try
            {

                var transactions = (await unitOfWork.TransactionRepository.GetAll()).ToList();
                var listOfTransaction = new List<TransactionDTO>();
                foreach (var transaction in transactions)
                {
                    var mappedTransaction = mapper.Map<TransactionDTO>(transaction);
                    mappedTransaction.DestinationAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountDestinationId);
                    mappedTransaction.SourceAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountSourceId);
                    listOfTransaction.Add(mappedTransaction);

                }

                return listOfTransaction.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllTransactions)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task<TransactionDTO> GetTransactionById(int id)
        {
            try
            {

                var transactionFinded = await unitOfWork.TransactionRepository.GetById(id);
                return mapper.Map<TransactionDTO>(transactionFinded);
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetTransactionById)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }

        public async Task UpdateTransaction(TransactionDTO transaction)
        {
            try
            {
                var transactionMapped = mapper.Map<Transaction>(transaction);
                unitOfWork.TransactionRepository.Update(transactionMapped);
                await unitOfWork.Save();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(UpdateTransaction)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }
        public async Task<int> TransActionCountByData(DateTime dataOfTrnsaction)
        {
            try
            {
                return (await unitOfWork.TransactionRepository.GetAll()).Where(e => e.DateOftransfering == dataOfTrnsaction).Count();
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(TransActionCountByData)} in class {this.GetType()}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }


        public async Task ExecuteTransaction(int accountSourceId, string accountDestinationNumber, decimal amount)
        {
            using (var trans = unitOfWork.BeginTransaction())
            {
                try
                {
                    var source = (await unitOfWork.AccountRepository.GetAll()).Where(a => a.Id == accountSourceId).FirstOrDefault();
                    var destination = (await unitOfWork.AccountRepository.GetAll()).Where(a => a.Number == accountDestinationNumber).FirstOrDefault();
                    if (source == null) throw new ValidationException("Source account is not founded", "");
                    if (destination == null) throw new ValidationException("Destination account is not founded", "");
                    if ((source.Balance -= amount) >= 0)
                    {
                        source.Balance -= amount;
                        destination.Balance += amount;
                        var transaction = new TransactionDTO()
                        {
                            AccountSourceId = accountSourceId,
                            AccountDestinationId = destination.Id,
                            DateOftransfering = DateTime.Now,
                            Amount = amount
                        };

                        await AddTransaction(transaction);
                        unitOfWork.CommitTransaction();

                    }
                    else
                    {
                        throw new ValidationException("Not enough money on your account", "");
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError($"Catch an exception in method {nameof(ExecuteTransaction)} in class {this.GetType()}. The exception is {ex.Message}. " +
                       $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                    unitOfWork.RollbackTransaction();
                    throw ex;
                }

            }
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }


        public async Task<IEnumerable<TransactionDTO>> GetAllUserTransactions(ClaimsPrincipal user)
        {
            try
            {
                var id = unitOfWork.UserManager.GetUserId(user);
                var personFinded = (await unitOfWork.PersonRepository.GetAll()).Where(e => e.ApplicationUserFkId == id).FirstOrDefault();
                var userAccounts = (await unitOfWork.PersonRepository.GetById(personFinded.Id)).Accounts;
                var listOfTransaction = new List<TransactionDTO>();
                foreach (var account in userAccounts)
                {
                    foreach (var trans in await unitOfWork.TransactionRepository.GetAll())
                    {
                        if (trans != null)
                        {
                            if (trans.AccountSourceId == account.Id || trans.AccountDestinationId == account.Id)
                            {
                                var mappedTransaction = mapper.Map<TransactionDTO>(trans);
                                mappedTransaction.DestinationAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountDestinationId);
                                mappedTransaction.SourceAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountSourceId);
                                listOfTransaction.Add(mappedTransaction);
                            }
                        }

                    }

                }
                return listOfTransaction;
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception in method {nameof(GetAllUserTransactions)}. The exception is {ex.Message}. " +
                   $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
                throw ex;

            }
        }
    }
}


//foreach (var account in userAccounts)
//{
//    var transactions = account.Transactions;
//    if (transactions != null)
//    {
//        listOfTransaction.AddRange(transactions.Select(t => mapper.Map<TransactionDTO>(t)));
//    }

//}