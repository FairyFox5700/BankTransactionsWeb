using BankTransaction.BAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.DAL.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
{
    public class JWTSecurityService: IJwtSecurityService
    {
        private readonly BankTransactionContext context;
        private readonly IConfiguration config;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<JWTSecurityService> logger;

        public JWTSecurityService(BankTransactionContext context, IConfiguration config, IUnitOfWork unitOfWork, ILogger<JWTSecurityService> logger)
        {
            this.context = context;
            this.config = config;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<string> GenerateJWTToken(string email)
        {
            try
            {
                var user = await unitOfWork.UserManager.FindByEmailAsync(email);
                var roles = await unitOfWork.UserManager.GetRolesAsync(user);
                var claims = new List<Claim>
                {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim( JwtRegisteredClaimNames.Aud, config["Jwt:Audience"]),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
               new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).
               ToUnixTimeSeconds().ToString()),
               new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1))
               .ToUnixTimeSeconds().ToString())
                };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var token = new JwtSecurityToken(
                    new JwtHeader
                    (
                        new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                        SecurityAlgorithms.HmacSha256)),
                    new JwtPayload(claims));

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exeptoin {ex} witth inner {ex.InnerException}");
            }
            return null;

        }
    }
}
