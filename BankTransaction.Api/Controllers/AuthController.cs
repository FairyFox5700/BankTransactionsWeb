﻿using AutoMapper;
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
using BankTransaction.Models.Validation;

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
        public async Task<ApiResponse<AuthSuccesfullModel>> Login([FromBody]RequestLoginModel model)
        {
            //modelState
            var person = PersonDtoToRequestLoginMapper.Instance.MapBack(model);
            var result = await authService.LoginPerson(person);
            if (result.Success)
            {
                return new ApiResponse<AuthSuccesfullModel>(new AuthSuccesfullModel
                {
                    Token = result.Token,

                    RefreshToken = result.RefreshToken
                });
            }
            else
            {
                return ApiResponse<AuthSuccesfullModel>.BadRequest;
            }
        }

        [HttpPost]
        [Route("refreshToken")]
        public async Task<ApiResponse<AuthSuccesfullModel>> RefreshToken([FromBody]AuthSuccesfullModel model )
        {

            var tokenDto = RefreshTokenDtoToAuthSuccesMapper.Instance.MapBack(model);
            var result = await jwtService.RefreshToken(tokenDto);
            if (result.Success)
            {
                return new ApiResponse<AuthSuccesfullModel>(new AuthSuccesfullModel
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken
                });
            }
            else
            {
                return new ApiResponse<AuthSuccesfullModel> (400, new ApiErrorResponse() { Message = result.GetErrors});
            }
        }


        [HttpPost]
        [Route("register")]
        //[ServiceFilter(typeof(ValidationFilter))]
        public async Task<ApiResponse<AuthResult>> Register([FromBody] RequestRegisterModel model)
        {
            //if (!ModelState.IsValid)
            //    return  BadRequest(  new ApiErrorResonse() { ValidationErrors = ModelState.GetErrors().ToList() });
            var person = PersonDTOToRequestRegisterMapper.Instance.MapBack(model);
            var result = await authService.RegisterPersonWithJwtToken(person);
            if (result.Success)
            {
                return new ApiResponse<AuthResult>(result);
            }
            else
            {
                return new ApiResponse<AuthResult>(400, new ApiErrorResponse() { Message = result.GetErrors});
            }

        }

    }
}