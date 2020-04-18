using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IJwtSecurityService
    {
        Task<string> GenerateJWTToken(string email);
    }
}
