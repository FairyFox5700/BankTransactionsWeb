using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Implementation.Repositories.EFRepositories
{
    public class TokenRepository : BaseRepository<RefreshToken>, ITokenRepository
    {
        private readonly BankTransactionContext context;

        public TokenRepository(BankTransactionContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<RefreshToken> GetRefreshTokenForCurrentToken(string token)
        {
            return await  context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == token);
        }
    }
}
