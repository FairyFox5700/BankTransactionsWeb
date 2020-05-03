using BankTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Abstract
{
    public interface ITokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetRefreshTokenForCurrentToken(string token);
    }
}
