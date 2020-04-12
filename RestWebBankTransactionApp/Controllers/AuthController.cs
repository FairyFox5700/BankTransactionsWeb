using AutoMapper;
using BankTransactionWeb.BAL.Interfaces;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestWebBankTransactionApp.Models;
using System.Threading.Tasks;

namespace RestWebBankTransactionApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IAuthenticationService authService;
        private readonly IJwtSecurityService jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, IMapper mapper, IAuthenticationService service, IJwtSecurityService JwtService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.authService = service;
            jwtService = JwtService;
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            var person = mapper.Map<PersonDTO>(model);
            var result = await authService.LoginPerson(person);
            if (result.Succeeded)
            {
                return Ok(await jwtService.GenerateJWTToken(model.Email));
            }
            if (result == null)
            {
                return BadRequest( "You must confirm your email.");
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }




        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var person = mapper.Map<PersonDTO>(model);
            var result = await authService.RegisterPerson(person);
            if (result == null)
            {
                BadRequest("Please confirm your email");

            }
            if (result.Succeeded)
            {
                return Ok(await jwtService.GenerateJWTToken(model.Email));
            }
            else
            {
                return BadRequest("Invalid register attempt");
            }

        }

    }
}