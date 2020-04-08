using BankTransactionWeb.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class AdminService
    {
        private readonly ILogger<AdminService> logger;

        public AdminService(ILogger<AdminService> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
        }
    }
}
