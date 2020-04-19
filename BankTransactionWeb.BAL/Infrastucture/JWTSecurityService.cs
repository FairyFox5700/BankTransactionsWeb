﻿using BankTransaction.BAL.Abstract;
using BankTransaction.DAL.Abstract;
using BankTransaction.DAL.Implementation.Core;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
{
    public class JWTSecurityService : IJwtSecurityService
    {
        private readonly BankTransactionContext context;
        private readonly IConfiguration config;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<JWTSecurityService> logger;
        private readonly JwtSettings jwtSettings;
        private readonly TokenValidationParameters tokenValidationParameters;

        public JWTSecurityService(BankTransactionContext context, IConfiguration config, IUnitOfWork unitOfWork, ILogger<JWTSecurityService> logger, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters)
        {
            this.context = context;
            this.config = config;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.jwtSettings = jwtSettings;
            this.tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthResult> GenerateJWTToken(string email)
        {
            try
            {
                var user = await unitOfWork.UserManager.FindByEmailAsync(email);
                var roles = await unitOfWork.UserManager.GetRolesAsync(user);
                var key = Encoding.UTF8.GetBytes(config[jwtSettings.Key]);
                var claims = new List<Claim>
                {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim( JwtRegisteredClaimNames.Aud, config[jwtSettings.Audience]),
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
                var tokeDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifeTime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var tokenJwtHandler = new JwtSecurityTokenHandler();
                var token = tokenJwtHandler.CreateJwtSecurityToken(tokeDescription);
                var refreshToken = new RefreshToken
                {
                    JwtId = token.Id,
                    AppUserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExpieryDate = DateTime.UtcNow.AddHours(4)
                };
                unitOfWork.TokenRepository.Add(refreshToken);
                await unitOfWork.Save();
                // await unitOfWork.RefreshTokenAddAync;
                return new AuthResult
                {
                    Token = tokenJwtHandler.WriteToken(token),
                    Errors = null,
                    Success = true,
                    RefreshToken = refreshToken.Token
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Exeptoin {ex} witth inner {ex.InnerException}");
            }
            return null;



        }


        public async Task<AuthResult> RefreshToken(RefreshTokenDTO model)
        {
            var principal = GetCalimpPrincipalFromExpiredToken(model.RefreshToken);

            if (principal == null)
            {
                return new AuthResult()
                {
                    Errors = new[] { "Token validation failed" }
                };


            }
            var expieryUnixDate = long.Parse(principal.Claims.SingleOrDefault(x => x.Type ==
               JwtRegisteredClaimNames.Exp).Value);
            var expieryDateInUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expieryUnixDate)
                .Subtract(jwtSettings.TokenLifeTime);
            if (expieryDateInUtc > DateTime.UtcNow)
            {
                return new AuthResult()
                {
                    Errors = new[] { "Token has not expired yet" }
                };
            }
            else
            {
                var tokenIdentifier = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                var refreshToken = await unitOfWork.TokenRepository.GetRefreshTokenForCurrentToken(model.Token);
                if (refreshToken == null)
                {
                    return new AuthResult { Errors = new[] { "This refresh token does not exists" } };
                }
                if (DateTime.UtcNow > refreshToken.ExpieryDate)
                {
                    return new AuthResult()
                    { Errors = new[] { "Refresh token has expiered" } };

                }
                if (refreshToken.IsInvalidated)
                {
                    return new AuthResult()
                    {
                        Errors = new[] { "Current token is invalidated" }
                    };
                }
                if (refreshToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Errors = new[] { "Current tokenhas alreadu used" }
                    };
                }

                if (refreshToken.JwtId != tokenIdentifier)
                {
                    return new AuthResult()
                    {
                        Errors = new[] { "Refresh token jwt identifier is not match with this token id" }
                    };
                }
                refreshToken.IsUsed = true;
                unitOfWork.TokenRepository.Update(refreshToken);
                await unitOfWork.Save();
                var user = await unitOfWork.UserManager
                   .FindByIdAsync(principal.Claims
                   .Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
                return await GenerateJWTToken(user.Email);


            }
        }



        private ClaimsPrincipal GetCalimpPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validToken);
                if (!IsJwtSecurityAlgorithmValid(validToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception ex)
            {
                logger.LogError($"Catch an exception {ex} while token validatioon");
                return null;
            }
        }

        private bool IsJwtSecurityAlgorithmValid(SecurityToken token)
        {
            var jwtSecurityToken = token as JwtSecurityToken;
            return (jwtSecurityToken != null) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
