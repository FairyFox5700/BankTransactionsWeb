using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BankTransaction.Api.Models;
using System.Threading.Tasks;
using System.Linq;
using BankTransaction.Api.Models;
using BankTransaction.Models.DTOModels;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;

namespace BankTransaction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [ServiceFilter(typeof(ValidationFilter))]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IJwtAuthenticationService authService;
        private readonly IJwtSecurityService jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, IMapper mapper, IJwtAuthenticationService service, IJwtSecurityService jwtService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.authService = service;
            this.jwtService = jwtService;
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]RequestLoginModel model)
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
            //var tokenDto = new RefreshTokenDTO { Token = model.Token, RefreshToken = model.RefreshToken };
            var tokenDto = mapper.Map<RefreshTokenDTO>(model);
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
        //[ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> Register([FromBody] RequestRegisterModel model)
        {
            //if (!ModelState.IsValid)
            //    return  BadRequest(  new ApiErrorResonse() { ValidationErrors = ModelState.GetErrors().ToList() });
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