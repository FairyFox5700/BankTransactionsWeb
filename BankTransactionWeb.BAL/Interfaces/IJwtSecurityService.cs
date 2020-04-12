using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
    public interface IJwtSecurityService
    {
        Task<string> GenerateJWTToken(string email);
    }
}
