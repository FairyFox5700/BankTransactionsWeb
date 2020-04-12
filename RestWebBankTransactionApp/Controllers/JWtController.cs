using BankTransactionWeb.DAL.EfCoreDAL.EfCore;
using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestWebBankTransactionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWtController : ControllerBase
    {
        private readonly BankTransactionContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public JWtController(BankTransactionContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [Route("/token")]
        public async Task<IActionResult> Token(string userName, string password)
        {
            if (await CheckPasswordAndUserName(userName, password))
            {
                return new ObjectResult(await GenerateJWTToken(userName));
            }
            else
                return BadRequest();
        }

        public async Task<bool> CheckPasswordAndUserName(string userName, string password)
        {
            var user = await userManager.FindByEmailAsync(userName);
            return await userManager.CheckPasswordAsync(user, password);
        }

        private async Task<dynamic> GenerateJWTToken(string userName)
        {
            var user = await userManager.FindByEmailAsync(userName);
            var roles = context.UserRoles.Where(e => e.UserId == user.Id)
                .Select(e => new { e.UserId, e.RoleId, context.Roles.Where(r => r.Id == e.RoleId).FirstOrDefault().Name });
            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.Name, userName),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).
               ToUnixTimeSeconds().ToString()),
               new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1))
               .ToUnixTimeSeconds().ToString())
           };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            var token = new JwtSecurityToken(
                new JwtHeader
                (
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyToGEnerateTokeJWT_DotC")),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));
            var output = new

            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = userName
            };
            return output;
        }



    }
}