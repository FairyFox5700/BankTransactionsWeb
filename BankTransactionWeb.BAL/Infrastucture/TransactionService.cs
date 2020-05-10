using BankTransaction.BAL.Abstract;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Models;
using BankTransaction.Models.Mapper;
using BankTransaction.Models.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Mapper.MpaperOld;

namespace BankTransaction.BAL.Implementation.Infrastucture
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<TransactionService> logger;

        public TransactionService(IUnitOfWork unitOfWork, ILogger<TransactionService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }
        public async Task AddTransaction(TransactionDTO transaction)
        {
            var transactionMapped = TransactionEntityToDtoMapper.Instance.MapBack(transaction);
            transactionMapped.DateOftransfering = DateTime.Now;
            unitOfWork.TransactionRepository.Add(transactionMapped);
            await unitOfWork.Save();
        }

        public async Task DeleteTransaction(TransactionDTO transaction)
        {

            var transactionMapped = TransactionEntityToDtoMapper.Instance.MapBack(transaction);
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
                    var mappedTransaction = TransactionEntityToDtoMapper.Instance.Map(transaction);
                    mappedTransaction.DestinationAccountNumber = (await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountDestinationId))?.Number;
                    //mappedTransaction.SourceAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountSourceId);
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
                var transaction= TransactionEntityToDtoMapper.Instance.Map(transactionFinded);
                //transaction.SourceAccount = (await unitOfWork.AccountRepository.GetById(transactionFinded.AccountSourceId));
                transaction.DestinationAccountNumber = (await unitOfWork.AccountRepository.GetById(transactionFinded.AccountDestinationId))?.Number;
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<ValidationModel> UpdateTransaction(TransactionDTO transaction)
        {
            try
            {
                var transactionMapped = TransactionEntityToDtoMapper.Instance.MapBack(transaction);
                var source = (await unitOfWork.AccountRepository.GetTransactionByDestinationNumber(transaction.SourceAccountNumber));
                var destination = (await unitOfWork.AccountRepository.GetTransactionByDestinationNumber(transaction.DestinationAccountNumber));
                if (source == null) return new ValidationModel("Source account is not founded", true);
                if (destination == null) return new ValidationModel("Destination account is not founded", true);
                transactionMapped.AccountDestinationId = destination.Id;
                transactionMapped.AccountSourceId = source.Id;
                unitOfWork.TransactionRepository.Update(transactionMapped);
                await unitOfWork.Save();
                return new ValidationModel("Successfully updated", true);
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


        public async Task<ValidationModel> ExecuteTransaction(int accountSourceId, string accountDestinationNumber, decimal amount)
        {
            using (var trans = unitOfWork.BeginTransaction())
            {
                try
                {
                    var source = (await unitOfWork.AccountRepository.GetById(accountSourceId));
                    var destination = (await unitOfWork.AccountRepository.GetTransactionByDestinationNumber(accountDestinationNumber));
                    if (source == null) return new ValidationModel ("Source account is not founded", true);
                    if (destination == null) return new ValidationModel("Destination account is not founded", true);
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
                        return new ValidationModel($"You have succesfully send {amount} to  {accountDestinationNumber} account.", false);

                    }
                    else
                    {
                        return new ValidationModel("Not enough money on your account", true);
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
                            var mappedTransaction = TransactionEntityToDtoMapper.Instance.Map(trans);
                            mappedTransaction.DestinationAccountNumber =( await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountDestinationId))?.Number;
                            //mappedTransaction.SourceAccount = await unitOfWork.AccountRepository.GetById(mappedTransaction.AccountSourceId);
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