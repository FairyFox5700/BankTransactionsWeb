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
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task AddTransaction(TransactionDTO transaction)
        {
            var transactionMapped = mapper.Map<Transaction>(transaction);
            unitOfWork.TransactionRepository.Add(transactionMapped);
            await unitOfWork.Save();
        }

        public async Task DeleteTransaction(TransactionDTO transaction)
        {
            var transactionMapped = mapper.Map<Transaction>(transaction);
            unitOfWork.TransactionRepository.Delete(transactionMapped);
            await unitOfWork.Save();
        }

        public async Task<IEnumerable<TransactionDTO>> GetAllCompanies()
        {
            var transactions = await unitOfWork.TransactionRepository.GetAll();
            return transactions.Select(product => mapper.Map<TransactionDTO>(transactions));
        }

        public async Task<TransactionDTO> GetTransactionById(int id)
        {
            var transactionFinded = await unitOfWork.TransactionRepository.GetById(id);
            return mapper.Map<TransactionDTO>(transactionFinded);
        }

        public async Task UpdateTransaction(TransactionDTO transaction)
        {
            var transactionMapped = mapper.Map<Transaction>(transaction);
            unitOfWork.TransactionRepository.Update(transactionMapped);
            await unitOfWork.Save();
        }
        public async Task<int> TransActionCountByData(DateTime dataOfTrnsaction)
        {
            return (await unitOfWork.TransactionRepository.GetAll()).Where(e => e.DateOftransfering == dataOfTrnsaction).Count();
        }
    }
}
