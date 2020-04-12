﻿using AutoMapper;
using BankTransactionWeb.BAL.Cofiguration;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
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
                    await AddTransaction(new TransactionDTO() { 
                        AccountSourceId = accountSourceId, AccountDestinationId = destination.Id, DateOftransfering = DateTime.Now, Amount = amount });
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
                throw ex;

            }
           
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }


        public async Task<IEnumerable<TransactionDTO>> GetAllUserTransactions(int userId)
        {
            try
            {

                var userAccounts = (await unitOfWork.PersonRepository.GetById(userId)).Accounts;
                var listOfTransaction = new List<TransactionDTO>();
                foreach (var account in userAccounts)
                {
                    var transactions = account.Transactions;
                    listOfTransaction.AddRange(transactions.Select(t => mapper.Map<TransactionDTO>(t)));
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
