
using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface ISender 
    {
        Task SendEmailAsync(CustomMessage message);
    }
}
