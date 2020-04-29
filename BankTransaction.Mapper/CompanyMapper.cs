using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Web.ViewModel;
using BankTransaction.Configuration;

namespace BankTransaction.Mapper
{
    public class PersonMapperAddModel : IMapper<PersonDTO, AddPersonViewModel>
    {
        public AddPersonViewModel Map(PersonDTO source)
        {
            return new AddPersonViewModel()
            {
                DataOfBirth = source.DataOfBirth,
                LastName = source.LastName,
                Surname = source.Surname,
                Name = source.Name
            };
        }

        public PersonDTO MapBack(AddPersonViewModel destination)
        {
            return new PersonDTO()
            {
                DataOfBirth = destination.DataOfBirth,
                LastName = destination.LastName,
                Surname = destination.Surname,
                Name = destination.Name
            };
        }
    }
    public class PersonMapperUpdateModel : IMapper<PersonDTO, UpdatePersonViewModel>
    {
        public UpdatePersonViewModel Map(PersonDTO source)
        {
            return new UpdatePersonViewModel()
            {
                Id = source.Id,
                ApplicationUserFkId = source.ApplicationUserFkId,
                PhoneNumber = source.PhoneNumber,
                UserName = source.UserName,
                DataOfBirth = source.DataOfBirth,
                LastName = source.LastName,
                Surname = source.Surname,
                Name = source.Name
            };
        }

        public PersonDTO MapBack(UpdatePersonViewModel destination)
        {
            return new PersonDTO()
            {
                Id = destination.Id,
                ApplicationUserFkId = destination.ApplicationUserFkId,
                PhoneNumber = destination.PhoneNumber,
                UserName = destination.UserName,
                DataOfBirth = destination.DataOfBirth,
                LastName = destination.LastName,
                Surname = destination.Surname,
                Name = destination.Name
            };
        }
    }

    public class AccountMapperAddModel : IMapper<AccountDTO, AddAccountViewModel>
    {
        public AddAccountViewModel Map(AccountDTO source)
        {
            return new AddAccountViewModel()
            {
                Balance = source.Balance,
                Number = source.Number,
                PersonId = source.PersonId,
            };
        }

        public AccountDTO MapBack(AddAccountViewModel destination)
        {
            return new AccountDTO()
            {
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
            };
        }
    }


    public class AccountMapperUpdateModel : IMapper<AccountDTO, UpdateAccountViewModel>
    {

        public UpdateAccountViewModel Map(AccountDTO source)
        {
            return new UpdateAccountViewModel()
            {
                Id = source.Id,
                Balance = source.Balance,
                Number = source.Number,
                PersonId = source.PersonId,
            };
        }


        public AccountDTO MapBack(UpdateAccountViewModel destination)
        {
            return new AccountDTO()
            {
                Id = destination.Id,
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
            };
        }
    }


    public class CompanyMapperUpdateModel : IMapper<UpdateCompanyViewModel, CompanyDTO>
    {
        public CompanyDTO Map(UpdateCompanyViewModel source)
        {
            return new CompanyDTO()
            {
                Id = source.Id,
                DateOfCreation = source.DateOfCreation,
                Name = source.Name
            };
        }

        public UpdateCompanyViewModel MapBack(CompanyDTO destination)
        {
            return new UpdateCompanyViewModel()
            {
                Id = destination.Id,
                DateOfCreation = destination.DateOfCreation,
                Name = destination.Name
            };
        }
    }

    public class CompanyMapperAddModel : IMapper<AddCompanyViewModel, CompanyDTO>
    {
        public CompanyDTO Map(AddCompanyViewModel source)
        {
            return new CompanyDTO()
            {
                DateOfCreation = source.DateOfCreation,
                Name = source.Name
            };
        }

        public AddCompanyViewModel MapBack(CompanyDTO destination)
        {
            return new AddCompanyViewModel()
            {
                DateOfCreation = destination.DateOfCreation,
                Name = destination.Name
            };
        }
    }

    public class ShareholderMapperAddModel : IMapper<AddShareholderViewModel, ShareholderDTO>
    {
        public ShareholderDTO Map(AddShareholderViewModel source)
        {
            return new ShareholderDTO()
            {
                Company = source.Company,
                CompanyId = source.CompanyId,
                Person = source.Person,
                PersonId = source.PersonId,
            };
        }

        public AddShareholderViewModel MapBack(ShareholderDTO destination)
        {
            return new AddShareholderViewModel()
            {
                Company = destination.Company,
                CompanyId = destination.CompanyId,
                Person = destination.Person,
                PersonId = destination.PersonId,
            };
        }
    }

    public class ShareholderMapperUpdateModel : IMapper<UpdateShareholderViewModel, ShareholderDTO>
    {
        public ShareholderDTO Map(UpdateShareholderViewModel source)
        {
            return new ShareholderDTO()
            {
                Id = source.Id,
                Company = source.Company,
                CompanyId = source.CompanyId,
                Person = source.Person,
                PersonId = source.PersonId,
            };
        }

        public UpdateShareholderViewModel MapBack(ShareholderDTO destination)
        {
            return new UpdateShareholderViewModel()
            {
                Id = destination.Id,
                Company = destination.Company,
                CompanyId = destination.CompanyId,
                Person = destination.Person,
                PersonId = destination.PersonId,
            };
        }
    }

    public class TransactionMapperList : IMapper<TransactionDTO, TransactionListViewModel>
    {
        public TransactionListViewModel Map(TransactionDTO source)
        {
            return new TransactionListViewModel()
            {
                Id = source.Id,
                DateOftransfering = source.DateOftransfering,
                AccountSourceId = source.AccountSourceId,
                AccountDestinationeNumber = source.DestinationAccount?.Number,
                AccountSourceNumber = source.SourceAccount?.Number,
                Amount = source.Amount

            };
        }

        public TransactionDTO MapBack(TransactionListViewModel destination)
        {
            return new TransactionDTO()
            {
                Id = destination.Id,
                DateOftransfering = destination.DateOftransfering,
                AccountSourceId = destination.AccountSourceId,
                //AccountDestinationeNumber = source.DestinationAccount?.Number,
                //AccountSourceNumber = source.SourceAccount?.Number,
                Amount = destination.Amount

            };
        }
    }


    public class TransactionMapperUpdateModel : IMapper<TransactionDTO, UpdateTransactionViewModel>
    {
        public UpdateTransactionViewModel Map(TransactionDTO source)
        {
            return new UpdateTransactionViewModel()
            {
                Id = source.Id,
                //AccountDestinationId = source.AccountDestinationId,
                AccountSourceId = source.AccountSourceId,
                Amount = source.Amount
            };
        }

        public TransactionDTO MapBack(UpdateTransactionViewModel destination)
        {
            return new TransactionDTO()
            {
                Id = destination.Id,
                //AccountDestinationId = destination.AccountDestinationId,
                AccountSourceId = destination.AccountSourceId,
                Amount = destination.Amount

            };
        }
    }

    public class TransactionMapperAddModel : IMapper<TransactionDTO, AddTransactionViewModel>
    {
        public AddTransactionViewModel Map(TransactionDTO source)
        {
            return new AddTransactionViewModel()
            {
                AccountDestinationId = source.AccountDestinationId,
                AccountSourceId = source.AccountSourceId,
                Amount = source.Amount,
            };
        }

        public TransactionDTO MapBack(AddTransactionViewModel destination)
        {
            return new TransactionDTO()
            {
                AccountDestinationId = destination.AccountDestinationId,
                AccountSourceId = destination.AccountSourceId,
                Amount = destination.Amount

            };
        }
    }

    public class LoginMapper : IMapper<LoginViewModel, PersonDTO>
    {
        public PersonDTO Map(LoginViewModel source)
        {
            return new PersonDTO()
            {
                Email = source.Email,
                Password = source.Password,
                RememberMe = source.RememberMe
            };
        }

        public LoginViewModel MapBack(PersonDTO destination)
        {
            return new LoginViewModel()
            {
                Email = destination.Email,
                Password = destination.Password,
                RememberMe = destination.RememberMe
            };
        }
    }

    public class RegisterMapper : IMapper<RegisterViewModel, PersonDTO>
    {
        public PersonDTO Map(RegisterViewModel source)
        {
            return new PersonDTO()
            {
                ConfirmPassword = source.ConfirmPassword,
                DataOfBirth = source.DataOfBirth,
                Email = source.Email,
                LastName = source.LastName,
                Name = source.Name,
                Password = source.Password,
                PhoneNumber = source.PhoneNumber,
                Surname = source.Surname,
                UserName = source.UserName,
            };
        }

        public RegisterViewModel MapBack(PersonDTO destination)
        {
            return new RegisterViewModel()
            {
                ConfirmPassword = destination.ConfirmPassword,
                DataOfBirth = destination.DataOfBirth,
                Email = destination.Email,
                LastName = destination.LastName,
                Name = destination.Name,
                Password = destination.Password,
                PhoneNumber = destination.PhoneNumber,
                Surname = destination.Surname,
                UserName = destination.UserName,
            };

        }
    }

   

    public class RoleMapperAddModel : IMapper<RoleDTO, AddRoleViewModel>
    {
        public AddRoleViewModel Map(RoleDTO source)
        {
            return new AddRoleViewModel()
            {
                Name = source.Name
            };
        }

        public RoleDTO MapBack(AddRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Name = destination.Name
            };
        }
    }


    public class RoleMapperUpdateModel : IMapper<RoleDTO, UpdateRoleViewModel>
    {
        public UpdateRoleViewModel Map(RoleDTO source)
        {
            return new UpdateRoleViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                Users = source.Users
            };
        }

        public RoleDTO MapBack(UpdateRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Id = destination.Id,
                Name = destination.Name,
                Users = destination.Users
            };
        }
    }

    public class RoleMapperListModel : IMapper<RoleDTO, ListRoleViewModel>
    {
        public ListRoleViewModel Map(RoleDTO source)
        {
            return new ListRoleViewModel()
            {
                Id = source.Id,
                Name = source.Name,
            };
        }

        public RoleDTO MapBack(ListRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Id = destination.Id,
                Name = destination.Name,
            };
        }
    }

    public class RoleIsersMapperListModel : IMapper< RoleDTO, UsersInRoleViewModel>
    {
        public UsersInRoleViewModel Map(RoleDTO source)
        {
            return new UsersInRoleViewModel()
            {
                Id = source.Id,
                Name = source.Name,
            };
        }

        public RoleDTO MapBack(UsersInRoleViewModel destination)
        {
            return new RoleDTO()
            {
                Id = destination.Id,
                Name = destination.Name
            };
        }
    }

    public class ResetPasswordMapper : IMapper<ResetPasswordViewModel, PersonDTO>
    {
        public PersonDTO Map(ResetPasswordViewModel source)
        {
            return new PersonDTO()
            {
                ConfirmPassword = source.ConfirmPassword,
                Email = source.Email,
                Password = source.Password,
                Token = source.Token
            };
        }

        public ResetPasswordViewModel MapBack(PersonDTO destination)
        {
            return new ResetPasswordViewModel()
            {
                ConfirmPassword = destination.ConfirmPassword,
                Email = destination.Email,
                Password = destination.Password,
                Token = destination.Token
            };
        }
    }

   
    

    public class UsersRoleMapper : IMapper<PersonInRoleDTO, UsersInRoleViewModel>
    {
        public UsersInRoleViewModel Map(PersonInRoleDTO source)
        {
            return new UsersInRoleViewModel
            {
                Name = source.Name,
                Id = source.Id,
                IsSelected = source.IsSelected,
                AppUserId = source.AppUserId,
                Surname = source.Surname,
                LastName = source.LastName,
                UserName = source.UserName
            };

        }

        public PersonInRoleDTO MapBack(UsersInRoleViewModel destination)
        {
            return new PersonInRoleDTO()
            {
                Name = destination.Name,
                Id = destination.Id,
                IsSelected = destination.IsSelected,
                AppUserId = destination.AppUserId,
                Surname = destination.Surname,
                LastName = destination.LastName,
                UserName = destination.UserName
            };
        }
    }

    
}



