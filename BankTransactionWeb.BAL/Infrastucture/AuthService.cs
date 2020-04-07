using AutoMapper;
using BankTransactionWeb.BAL.Models;
using BankTransactionWeb.DAL.Entities;
using BankTransactionWeb.DAL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Infrastucture
{
    public class AuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<AuthService> logger;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        //public async Task  LogInUser(PersonDTO personDTO)
        //{
        //    ApplicationUser user = await unitOfWork.AppUserManager.FindByEmailAsync(personDTO.Email);


        //}
    }
}
