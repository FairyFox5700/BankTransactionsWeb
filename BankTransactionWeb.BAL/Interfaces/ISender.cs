using BankTransactionWeb.BAL.Infrastucture.MessageServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface ISender 
    {
        Task SendEmailAsync(CustomMessage message);
    }
}
