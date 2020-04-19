using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BankTransaction.Api.Models;
using System.Threading.Tasks;
using System.Linq;
using BankTransaction.Models.Validation;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Controllers
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
            //modelState
            var person = mapper.Map<PersonDTO>(model);
            var result = await authService.LoginPerson(person);
            if (result.Success)
            {
                return Ok(new AuthSuccesfullModel
                {
                    Token = result.Token,

                    RefreshToken = result.RefreshToken
                });
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]AuthSuccesfullModel model )
        {
            ///MAPPPPPPERPRPRPRPRPRPRRP
            var tokenDto = new RefreshTokenDTO { Token = model.Token, RefreshToken = model.RefreshToken };
            var result = await jwtService.RefreshToken(tokenDto);
            if (result.Success)
            {
                return Ok(new AuthSuccesfullModel
                {
                    Token = result.Token,

                    RefreshToken = result.RefreshToken
                });
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return  BadRequest(  new ApiErrorResonse() { ValidationErrors = ModelState.GetErrors().ToList() });
            var person = mapper.Map<PersonDTO>(model);
            var result = await authService.RegisterPersonWithJwtToken(person);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

    }
}