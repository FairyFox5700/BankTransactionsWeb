using AutoMapper;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BankTransaction.Api.Models;
using System.Threading.Tasks;
using System.Linq;

using BankTransaction.Models.DTOModels;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.Api.Models.Mapper;

namespace BankTransaction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [ServiceFilter(typeof(ValidationFilter))]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtAuthenticationService authService;
        private readonly IJwtSecurityService jwtService;

        public AuthController(UserManager<ApplicationUser> userManager,  IJwtAuthenticationService service, IJwtSecurityService jwtService)
        {
            this.userManager = userManager;
            this.authService = service;
            this.jwtService = jwtService;
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]RequestLoginModel model)
        {
            //modelState
            var person = PersonDtoToRequestLoginMapper.Instance.MapBack(model);
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

            var tokenDto = RefreshTokenDtoToAuthSuccesMapper.Instance.MapBack(model);
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
            var person = PersonDTOToRequestRegisterMapper.Instance.MapBack(model);
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