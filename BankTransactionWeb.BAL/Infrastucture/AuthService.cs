using AutoMapper;
using BankTransaction.DAL.Abstract;
using Microsoft.Extensions.Logging;

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
