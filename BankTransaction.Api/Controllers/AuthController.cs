using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Mapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Abstract.RestApi;
using BankTransaction.BAL.Implementation.RestApi;
using BankTransaction.Entities;
using BankTransaction.Models.Mapper;
using BankTransaction.Models.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
{
    // [ServiceFilter(typeof(ValidationFilter))]
    public class AuthController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtAuthenticationService authService;
        private readonly IJwtSecurityService jwtService;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtAuthenticationService service, IJwtSecurityService jwtService)//IJwtTokenManager jwtTokenManager,
        {
            this.userManager = userManager;
            this.authService = service;
            this.jwtService = jwtService;
        }



        [HttpPost]
        [Route("login")]
      
        public async Task<ApiDataResponse<AuthSuccesfullModel>> Login([FromBody]RequestLoginModel model)
        {
            //modelState
            var person = PersonDtoToRequestLoginMapper.Instance.MapBack(model);
            var result = await authService.LoginPerson(person);
           
            if (result.Success)
            {
                return new ApiDataResponse<AuthSuccesfullModel>(new AuthSuccesfullModel
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken
                });
            }
            else
            {
                var apiErrorResult = AuthResultToApiErrorResponceMapper.Instance.Map(result);
                return new ApiDataResponse<AuthSuccesfullModel>(200, apiErrorResult);
            }
        }

        //[HttpPost]
        //[Route("logout")]
        //public async Task<ApiDataResponse<AuthSuccesfullModel>> Logout()
        //{
        //    return ApiDataResponse<AuthSuccesfullModel>.NotFound;
        //}

        [HttpPost]
        [Route("refreshToken")]
        public async Task<ApiDataResponse<AuthSuccesfullModel>> RefreshToken([FromBody]AuthSuccesfullModel model)
        {

            var tokenDto = RefreshTokenDtoToAuthSuccesMapper.Instance.MapBack(model);
            var result = await jwtService.RefreshToken(tokenDto);
            if (result.Success)
            {
                return new ApiDataResponse<AuthSuccesfullModel>(new AuthSuccesfullModel
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken
                });
            }
            else
            {
                return new ApiDataResponse<AuthSuccesfullModel>(400, new ApiErrorResponse(message: result.Message,messageType:result.MessageType));
            }
        }

        [HttpPost("revokeToken")]
        public async Task<ApiDataResponse<bool>> RevokeRefreshToken([FromBody]AuthSuccesfullModel model)
        {
            var tokenDto = RefreshTokenDtoToAuthSuccesMapper.Instance.MapBack(model);
             var result = await  jwtService.RevokeRefreshToken(tokenDto);
            if(result.Success)
            {
                return new ApiDataResponse<bool>(true);
            }
            else
            {
                return new ApiDataResponse<bool>(400, new ApiErrorResponse(message: result.Message, messageType: result.MessageType));
            }

        }

        //[HttpPost("token/cancel")]
        //public async Task<ApiDataResponse<bool>> CancelAccessToken()
        //{
        //    await jwtTokenManager.DeactivateCurrentTokenAsync();
        //    return new ApiDataResponse<bool>(true);
        //}

      
        [HttpPost]
        [Route("register")]
        //[ServiceFilter(typeof(ValidationFilter))]
        public async Task<ApiDataResponse<AuthResult>> Register([FromBody] RequestRegisterModel model)
        {
            //if (!ModelState.IsValid)
            //    return  BadRequest(  new ApiErrorResonse() { ValidationErrors = ModelState.GetErrors().ToList() });
            var person = PersonDTOToRequestRegisterMapper.Instance.MapBack(model);
            var result = await authService.RegisterPersonWithJwtToken(person);
            if (result.Success)
            {
                return new ApiDataResponse<AuthResult>(result);
            }
            else
            {
                return new ApiDataResponse<AuthResult>(400, new ApiErrorResponse(message: result.Message, messageType: result.MessageType));
            }

        }

    }
}