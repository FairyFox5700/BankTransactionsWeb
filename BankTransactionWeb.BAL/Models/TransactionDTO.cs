﻿using BankTransactionWeb.DAL.Entities;
using System;

namespace BankTransactionWeb.BAL.Models
{
    public class TransactionDTO
    {
        public int AccountSourceId { get; set; }
        public Account SourceAccount { get; set; }
        public int AccountDestinationId { get; set; }
        public Account DestinationAccount { get; set; }
        public DateTime DateOftransfering { get; set; }
        public decimal Amount { get; set; }
    }
}