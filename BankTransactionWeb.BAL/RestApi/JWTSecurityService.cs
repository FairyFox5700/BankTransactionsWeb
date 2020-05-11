using BankTransaction.Api.Models;
using BankTransaction.BAL.Abstract;
using BankTransaction.DAL.Abstract;
using BankTransaction.Entities;
using BankTransaction.Models.DTOModels;
using BankTransaction.Models.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.BAL.Implementation.RestApi
{
    public class JWTSecurityService : IJwtSecurityService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<JWTSecurityService> logger;
        private readonly ISet<RefreshToken> refreshTokens = new HashSet<RefreshToken>();
        private readonly JwtSettings jwtSettings;
        private readonly TokenValidationParameters tokenValidationParameters;

        public JWTSecurityService(IUnitOfWork unitOfWork,
            ILogger<JWTSecurityService> logger, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters)
        {
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
                var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
                var claims = new List<Claim>
                {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim( JwtRegisteredClaimNames.Aud, jwtSettings.Audience),
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
                    Issuer = jwtSettings.Issuer,
                    Audience = jwtSettings.Audience,
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifeTime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };

                var tokenJwtHandler = new JwtSecurityTokenHandler();
                var token = tokenJwtHandler.CreateJwtSecurityToken(tokeDescription);
                var refreshToken = new RefreshToken
                {
                    TokenKey = Guid.NewGuid().ToString(),
                    JwtId = token.Id,
                    AppUserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    ExpieryDate = DateTime.UtcNow.AddHours(4)
                };
                unitOfWork.TokenRepository.Add(refreshToken);
                await unitOfWork.Save();
                return new AuthResult
                {
                    Token = tokenJwtHandler.WriteToken(token),
                    Message = null,
                    Success = true,
                    RefreshToken = refreshToken.TokenKey,
                    ExpieryDate = refreshToken.ExpieryDate
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
            var principal = await GetCalimpPrincipalFromExpiredToken(model.Token);

            if (principal == null)
            {
                return new AuthResult()
                {
                    Message = ErrorMessage.TokenValidationFailed.GetDescription(),
                    MessageType = nameof(ErrorMessage.EmailNotValid)
                };


            }
            var expieryUnixDate = long.Parse(principal.Claims.SingleOrDefault(x => x.Type ==
               JwtRegisteredClaimNames.Exp).Value);
            var expieryDateInUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expieryUnixDate);
            //TODO this
            if (expieryDateInUtc > DateTime.UtcNow)
            {
                return new AuthResult()
                {
                    Message = ErrorMessage.TokenNotExpired.GetDescription(),
                    MessageType = nameof(ErrorMessage.TokenNotExpired)
                };
            }
            else
            {
                var tokenIdentifier = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                var refreshToken = await unitOfWork.TokenRepository.GetRefreshTokenForCurrentToken(model.RefreshToken);
                var rezult = ValidateToken(refreshToken, tokenIdentifier);
                if (rezult == null)
                {
                    refreshToken.IsUsed = true;
                    unitOfWork.TokenRepository.Update(refreshToken);
                    await unitOfWork.Save();
                    var user = await unitOfWork.UserManager
                       .FindByIdAsync(principal.Claims
                       .Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    return await GenerateJWTToken(user.Email);
                }
                return rezult;

            }
        }


        private async Task<RefreshToken> GetRefreshToken(string token)//REFRESHTOKEN DTO??????
        {
            var principal = await GetCalimpPrincipalFromExpiredToken(token);
            if (principal == null) return null;
            var tokenIdentifier = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var refreshToken = refreshTokens.SingleOrDefault(x => x.JwtId == tokenIdentifier);
            return refreshToken;
        }


    public async Task<AuthResult> RevokeRefreshToken(RefreshTokenDTO model)
        {
            var refreshToken = await GetRefreshToken(model.Token);
            var rezult = ValidateToken(refreshToken, refreshToken.JwtId);//smth here
            if (rezult == null)
            {
                refreshToken.IsInvalidated = true;
                unitOfWork.TokenRepository.Update(refreshToken);
                await unitOfWork.Save();
                //return new AuthResult()
                //{
                //    Message = ErrorMessage.TokenIsInvalidated.GetDescription(),
                //    MessageType = nameof(ErrorMessage.TokenIsInvalidated)
                //};
            }
            return rezult;
        }
        private AuthResult ValidateToken(RefreshToken refreshToken, string tokenIdentifier)
        {
            if (refreshToken == null)
            {
                return new AuthResult
                {
                    Message = ErrorMessage.RefreshTokenNotExists.GetDescription() ,
                    MessageType = nameof(ErrorMessage.RefreshTokenNotExists)
                };
            }

            if (DateTime.UtcNow > refreshToken.ExpieryDate)
            {
                return new AuthResult()
                {
                    Message = ErrorMessage.TokenNotExpired.GetDescription(),
                    MessageType = nameof(ErrorMessage.TokenNotExpired)
                };

            }
            if (refreshToken.IsInvalidated)
            {
                return new AuthResult()
                {
                    Message = ErrorMessage.TokenIsInvalidated.GetDescription() ,
                    MessageType = nameof(ErrorMessage.TokenIsInvalidated)
                };
            }
            if (refreshToken.IsUsed)
            {
                return new AuthResult()
                {
                    Message = ErrorMessage.TokenIsUsed.GetDescription(),
                    MessageType = nameof(ErrorMessage.TokenIsUsed)
                };
            }

            if (refreshToken.JwtId != tokenIdentifier)
            {
                return new AuthResult()
                {
                    Message = ErrorMessage.TokenIdMismatch.GetDescription(),
                    MessageType = nameof(ErrorMessage.TokenIdMismatch)
                };
            }
            return null;
        }


        private async Task<ClaimsPrincipal> GetCalimpPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                //var tokenData = await unitOfWork.TokenRepository.GetRefreshTokenForCurrentToken(token);
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
