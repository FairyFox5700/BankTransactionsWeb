using BankTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.DAL.Implementation.InMemoryCore
{
    public class InMemoryContainer
    {
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Company> Companies { get; set; }
        public ICollection<Person> Persons { get; set; }
        public ICollection<Shareholder> Shareholders { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public InMemoryContainer()
        {
            var person1 = new Person { Id = 1, Name = "Andriy", Surname = "Kovalenko", LastName = "Volodymirovich", DataOfBirth = new DateTime(1990, 2, 23) };
            var person2 = new Person { Id = 2, Name = "Vasil", Surname = "Kondratyuk", LastName = "Volodymirovich", DataOfBirth = new DateTime(1930, 2, 23) };
            var person3 = new Person { Id = 3, Name = "Masha", Surname = "Koshova", LastName = "Olegivna", DataOfBirth = new DateTime(1987, 3, 27) };
            var account1 = new Account { Id = 1, Number = "9235286739025463", Balance = 23 };
            var account2 = new Account { Id = 2, Number = "3289456729015682", Balance = 2000 };
            var account3 = new Account { Id = 3, Number = "3782569106739028", Balance = 300 };
            var account4 = new Account { Id = 4, Number = "2345678567891253", Balance = 0 };
            person1.Accounts = new List<Account> { account1, account2 };
            person2.Accounts = new List<Account> { account3 };
            person3.Accounts = new List<Account> { account4 };
            Persons = new List<Person> { person1, person2, person3 };
            var company1 = new Company { Id = 1, Name = "АбстрактБанк", DateOfCreation = new DateTime(2001, 2, 3) };
            var company2 = new Company { Id = 2, Name = "Аваль", DateOfCreation = new DateTime(1997, 4, 7) };
            var shareHolder1 = new Shareholder { Id = 1, Person = person1, Company = company1 };
            var shareHolder2 = new Shareholder { Id = 2, Person = person2, Company = company1 };
            var shareHolder3 = new Shareholder { Id = 3, Person = person3, Company = company2 };
            company1.Shareholders = new List<Shareholder> { shareHolder1, shareHolder2 };
            company2.Shareholders = new List<Shareholder> { shareHolder3 };
            Companies = new List<Company> { company1, company2 };
            Shareholders = new List<Shareholder> { shareHolder1, shareHolder2, shareHolder3 };
            var transaction1 = new Transaction { Id = 1, Amount = 300, AccountSourceId = 1, AccountDestinationId = 2, DateOftransfering = new DateTime(2004, 8, 3)};
            var transaction2 = new Transaction { Id = 2, AccountSourceId = 1, AccountDestinationId = 2, Amount = 70, DateOftransfering = new DateTime(2006, 6, 3) };
            var transaction3 = new Transaction { Id = 3, AccountSourceId = 2, AccountDestinationId = 2, Amount = 70, DateOftransfering = new DateTime(2009, 8, 3) };
            var transaction4 = new Transaction { Id = 4, AccountSourceId = 1, AccountDestinationId = 2, Amount = 34, DateOftransfering = new DateTime(2004, 8, 3)};
            var transaction5 = new Transaction { Id = 5, AccountSourceId = 4, AccountDestinationId = 2, Amount = 900, DateOftransfering = new DateTime(2004, 8, 3) };
            var transaction6 = new Transaction { Id = 6, AccountSourceId = 3, AccountDestinationId = 2, Amount = 800, DateOftransfering = new DateTime(2004, 8, 3) };
            account1.Transactions = new List<Transaction> { transaction1, transaction2 };
            account2.Transactions= new List<Transaction> { transaction3, transaction4 };
            account3.Transactions = new List<Transaction> { transaction5 };
            account4.Transactions = new List<Transaction> { transaction6 };
            Accounts = new List<Account> { account1, account2, account3, account4 };
            Transactions = new List<Transaction> { transaction1, transaction2, transaction3, transaction3, transaction4, transaction5, transaction5, transaction6 };

        }
    }
}
