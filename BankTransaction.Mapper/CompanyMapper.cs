using BankTransaction.Web.Areas.Admin.Models.ViewModels;
using BankTransaction.Web.Areas.Identity.Models.ViewModels;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Entities;
using BankTransaction.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BankTransaction.Mapper
{
    public class CompanyMapper : IMapper<Company, CompanyDTO>
    {
        public CompanyDTO Map(Company source)
        {
            return new CompanyDTO()
            {
                Id = source.Id,
                DateOfCreation = source.DateOfCreation,
                Shareholders =  source.Shareholders.Select(sh => new ShareholderMapper().Map(sh)).ToList(),
                Name = source.Name
            };
        }

        public Company MapBack(CompanyDTO destination)
        {
            return new Company()
            {
                Id = destination.Id,
                DateOfCreation = destination.DateOfCreation,
                Shareholders = destination.Shareholders.Select(sh => new ShareholderMapper().MapBack(sh)).ToList(),
                Name = destination.Name
            };
        }
    }

    public class PersonMapper : IMapper<Person, PersonDTO>
    {

        public PersonDTO Map(Person source)
        {
            return new PersonDTO()
            {
                Id = source.Id,
                DataOfBirth = source.DataOfBirth,
                ApplicationUserFkId = source.ApplicationUserFkId,
                LastName = source.LastName,
                Name = source.Name,
                Surname = source.Surname
            };
        }

        public Person MapBack(PersonDTO destination)
        {
            return new Person()
            {
                Id = destination.Id,
                DataOfBirth = destination.DataOfBirth,
                ApplicationUserFkId = destination.ApplicationUserFkId,
                LastName = destination.LastName,
                Name = destination.Name,
                Surname = destination.Surname
            };
        }
    }



    public class AccountMapper : IMapper<Account, AccountDTO>
    {
        public AccountDTO Map(Account source)
        {
            return new AccountDTO()
            {
                Id = source.Id,
                Balance = source.Balance,
                Number = source.Number,
                Transactions =source.Transactions.Select(tr => new TransactionMapper().Map(tr)).ToList()
            };
        }

        public Account MapBack(AccountDTO destination)
        {
            return new Account()
            {
                Id = destination.Id,
                Balance = destination.Balance,
                Number = destination.Number,
                PersonId = destination.PersonId,
                Transactions =  destination.Transactions.Select(tr => new TransactionMapper().MapBack(tr)).ToList(),
            };
        }
    }

    public class ShareholderMapper : IMapper<Shareholder, ShareholderDTO>
    {
        public ShareholderDTO Map(Shareholder source)
        {
            return new ShareholderDTO()
            {
                Id = source.Id,
                Company = new CompanyMapper().Map(source.Company),
                Person = new PersonMapper().Map(source.Person),
                PersonId = source.PersonId,
                CompanyId = source.CompanyId
            };
        }

        public Shareholder MapBack(ShareholderDTO destination)
        {
            return new Shareholder()
            {
                Id = destination.Id,
                Company = new CompanyMapper().MapBack(destination.Company),
                Person = new PersonMapper().MapBack(destination.Person),
                PersonId = destination.PersonId,
                CompanyId = destination.CompanyId
            };
        }
    }

    public class TransactionMapper : IMapper<Transaction, TransactionDTO>
    {
        public TransactionDTO Map(Transaction source)
        {
            return new TransactionDTO()
            {
                Id = source.Id,
                AccountDestinationId = source.AccountDestinationId,
                AccountSourceId = source.AccountSourceId,
                DateOftransfering = source.DateOftransfering,
                Amount = source.Amount,
                SourceAccount = source.SourceAccount
            };
        }

        public Transaction MapBack(TransactionDTO destination)
        {
            return new Transaction()
            {
                Id = destination.Id,
                AccountDestinationId = destination.AccountDestinationId,
                AccountSourceId = destination.AccountSourceId,
                DateOftransfering = destination.DateOftransfering,
                Amount = destination.Amount,
                SourceAccount = destination.SourceAccount,
            };
        }
    }


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
                AccountDestinationId = source.AccountDestinationId,
                AccountSourceId = source.AccountSourceId,
                Amount = source.Amount
            };
        }

        public TransactionDTO MapBack(UpdateTransactionViewModel destination)
        {
            return new TransactionDTO()
            {
                Id = destination.Id,
                AccountDestinationId = destination.AccountDestinationId,
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

    public class ApplicationUserMapper : IMapper<ApplicationUser, PersonDTO>
    {
        public PersonDTO Map(ApplicationUser source)
        {
            return new PersonDTO()
            {
                ApplicationUserFkId = source.Id,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber
            };
        }

        public ApplicationUser MapBack(PersonDTO destination)
        {
            return new ApplicationUser()
            {
                Id = destination.ApplicationUserFkId,
                Email = destination.Email,
                PhoneNumber = destination.PhoneNumber
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

    public class IdentityRoleMapper : IMapper<IdentityRole, RoleDTO>
    {
        public RoleDTO Map(IdentityRole source)
        {
            return new RoleDTO()
            {
                Id = source.Id,
                Name = source.Name,
            };
        }

        public IdentityRole MapBack(RoleDTO destination)
        {
            return new IdentityRole()
            {
                Id = destination.Id,
                Name = destination.Name
            };
        }
    }

    public class personRoleMapper : IMapper<PersonInRoleDTO, IdentityRole>
    {
        public IdentityRole Map(PersonInRoleDTO source)
        {
            return new IdentityRole()
            {
                Name = source.Name,
                Id = source.Id
            };

        }

        public PersonInRoleDTO MapBack(IdentityRole destination)
        {
            return new PersonInRoleDTO()
            {
                Name = destination.Name,
                Id = destination.Id
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

    public class PersonRoleMapper : IMapper<PersonInRoleDTO, Person>
    {
        public Person Map(PersonInRoleDTO source)
        {
            return new Person()
            {
                ApplicationUserFkId = source.AppUserId,
                Id =Convert.ToInt32(source.Id),
                LastName = source.LastName,
                Name = source.Name,
                Surname = source.Surname,
            };

        }

        public PersonInRoleDTO MapBack(Person destination)
        {
            return new PersonInRoleDTO()
            {
                AppUserId = destination.ApplicationUserFkId,
                LastName = destination.LastName,
                Name = destination.Name,
                Surname = destination.Surname,
                UserName = destination.ApplicationUser?.UserName,

            };
        }


    }
}



