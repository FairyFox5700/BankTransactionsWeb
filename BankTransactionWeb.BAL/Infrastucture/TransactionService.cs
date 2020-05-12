using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Models;
using BankTransaction.Models.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var transactionMapped = mapper.Map<Transaction>(transaction);
            transactionMapped.DateOftransfering = DateTime.Now;
            unitOfWork.TransactionRepository.Add(transactionMapped);
            await unitOfWork.Save();
        }

        public async Task DeleteTransaction(TransactionDTO transaction)
        {

            var transactionMapped = mapper.Map<Transaction>(transaction);
            unitOfWork.TransactionRepository.Delete(transactionMapped);
            await unitOfWork.Save();

        }


        public async Task<PaginatedModel<TransactionDTO>> GetAllTransactions(int pageNumber, int pageSize)
        {
            try
            {
                var transactions = (await unitOfWork.TransactionRepository.GetAll(pageNumber, pageSize));
                var listOfTransaction = new List<TransactionDTO>();
                foreach (var transaction in transactions)
                {
                    var mappedTransaction = mapper.Map<TransactionDTO>(transaction);
                    mappedTransaction.DestinationAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountDestinationId);
                    mappedTransaction.SourceAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountSourceId);
                    listOfTransaction.Add(mappedTransaction);
                }

                return new PaginatedModel<TransactionDTO>(listOfTransaction, transactions.PageNumber, transactions.PageSize, transactions.TotalCount, transactions.TotalPages);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<TransactionDTO> GetTransactionById(int id)
        {
            try
            {
                var transactionFinded = await unitOfWork.TransactionRepository.GetById(id);
                var transaction=mapper.Map<TransactionDTO>(transactionFinded);
                transaction.SourceAccount = (await unitOfWork.AccountRepository.GetById(transactionFinded.AccountSourceId));
                transaction.DestinationAccount = (await unitOfWork.AccountRepository.GetById(transactionFinded.AccountDestinationId));
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<ValidateTransactionModel> UpdateTransaction(TransactionDTO transaction)
        {
            try
            {
                var transactionMapped = mapper.Map<Transaction>(transaction);
                var source = (await unitOfWork.AccountRepository.GetTransactionByDestinationNumber(transaction.SourceAccount?.Number));
                var destination = (await unitOfWork.AccountRepository.GetTransactionByDestinationNumber(transaction.DestinationAccount?.Number));
                if (source == null) return new ValidateTransactionModel("Source account is not founded", true);
                if (destination == null) return new ValidateTransactionModel("Destination account is not founded", true);
                transactionMapped.AccountDestinationId = destination.Id;
                transactionMapped.AccountSourceId = source.Id;
                unitOfWork.TransactionRepository.Update(transactionMapped);
                await unitOfWork.Save();
                return new ValidateTransactionModel("Successfully updated", true);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        //public async Task<int> TransActionCountByData(DateTime dataOfTrnsaction)
        //{
        //    try
        //    {
        //        return (await unitOfWork.TransactionRepository.GetAll()).Where(e => e.DateOftransfering == dataOfTrnsaction).Count();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Catch an exception in method {nameof(TransActionCountByData)} in class {this.GetType()}. The exception is {ex.Message}. " +
        //           $"Inner exception {ex.InnerException?.Message ?? @"NONE"}");
        //        throw ex;

        //    }
        //}


        public async Task<ValidateTransactionModel> ExecuteTransaction(int accountSourceId, string accountDestinationNumber, decimal amount)
        {
            using (var trans = unitOfWork.BeginTransaction())
            {
                try
                {
                    var source = (await unitOfWork.AccountRepository.GetById(accountSourceId));
                    var destination = (await unitOfWork.AccountRepository.GetTransactionByDestinationNumber(accountDestinationNumber));
                    if (source == null) return new ValidateTransactionModel ("Source account is not founded", true);
                    if (destination == null) return new ValidateTransactionModel("Destination account is not founded", true);
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
                        return new ValidateTransactionModel($"You have succesfully send {amount} to  {accountDestinationNumber} account.", false);

                    }
                    else
                    {
                        return new ValidateTransactionModel("Not enough money on your account", true);
                    }

                }
                catch (Exception ex)
                {
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
                var personFinded = (await unitOfWork.PersonRepository.GetPersonByAccount(id));
                var userAccounts = (await unitOfWork.PersonRepository.GetById(personFinded.Id)).Accounts;
                var listOfTransaction = new List<TransactionDTO>();
                foreach (var account in userAccounts)
                {
                    foreach (var trans in await unitOfWork.TransactionRepository.GetAllTransactionsByAccountId(account.Id))
                    {
                        if (trans != null)
                        {
                            var mappedTransaction = mapper.Map<TransactionDTO>(trans);
                            mappedTransaction.DestinationAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountDestinationId);
                            mappedTransaction.SourceAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountSourceId);
                            listOfTransaction.Add(mappedTransaction);
                        }

                    }

                }
                return listOfTransaction;
            }
            catch (Exception ex)
            {
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