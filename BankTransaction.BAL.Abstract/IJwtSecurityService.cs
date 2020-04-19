using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Abstract
{
    public interface IJwtSecurityService
    {
        Task<AuthResult> RefreshToken(RefreshTokenDTO model);
        Task<AuthResult> GenerateJWTToken(string email);
    }
}
