using BankTransaction.Entities;
using Microsoft.EntityFrameworkCore;

using System;

namespace BankTransaction.DAL.Implementation.Core
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
            new Person { Id = 1, Name = "Андрій", Surname = "Kovalenko", LastName = "Volodimirovitch", DataOfBirth = new DateTime(1990, 2, 23) },
            new Person { Id = 2, Name = "Vasil", Surname = "Kondratyuk", LastName = "Volodymirovich", DataOfBirth = new DateTime(1930, 2, 23) },
            new Person { Id = 3, Name = "Masha", Surname = "Koshova", LastName = "Olegivna", DataOfBirth = new DateTime(1987, 3, 27) });
            modelBuilder.Entity<Account>().HasData(
            new Account { Id = 1, Number = "9235286739025463", Balance = 23, PersonId = 1 },
            new Account { Id = 2, Number = "3289456729015682", Balance = 2000, PersonId = 2 },
            new Account { Id = 3, Number = "3782569106739028", Balance = 300, PersonId = 2 },
            new Account { Id = 4, Number = "2345678567891253", Balance = 0, PersonId = 3 });
            modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Name = "AbstractBank", DateOfCreation = new DateTime(2001, 2, 3) },
            new Company { Id = 2, Name = "Aval", DateOfCreation = new DateTime(1997, 4, 7) });
            modelBuilder.Entity<Shareholder>().HasData(
            new Shareholder { Id = 1, PersonId = 1, CompanyId = 1 },
            new Shareholder { Id = 2, PersonId = 2, CompanyId = 1 },
            new Shareholder { Id = 3, PersonId = 3, CompanyId = 2 });
            modelBuilder.Entity<Transaction>().HasData(
            new Transaction { Id = 1, Amount = 300, AccountSourceId = 1, AccountDestinationId = 2, DateOftransfering = new DateTime(2004, 8, 3) },
            new Transaction { Id = 2, Amount = 70, AccountSourceId = 1, AccountDestinationId = 2, DateOftransfering = new DateTime(2006, 6, 3) },
            new Transaction { Id = 3, Amount = 70, AccountSourceId = 2, AccountDestinationId = 2, DateOftransfering = new DateTime(2009, 8, 3) },
            new Transaction { Id = 4, Amount = 34, AccountSourceId = 1, AccountDestinationId = 2, DateOftransfering = new DateTime(2004, 8, 3) },
            new Transaction { Id = 5, Amount = 900, AccountSourceId = 4, AccountDestinationId = 2, DateOftransfering = new DateTime(2004, 8, 3) },
            new Transaction { Id = 6, Amount = 800, AccountSourceId = 3, AccountDestinationId = 2, DateOftransfering = new DateTime(2004, 8, 3) });
        }
    }
}
