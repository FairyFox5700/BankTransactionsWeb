using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
   public interface IPersonService
    {
        Task<IEnumerable<PersonDTO>> GetAllPersons();
        Task<PersonDTO> GetPersonById(int id);
        void AddPerson(PersonDTO person);
        void UpdatePerson(PersonDTO person);
        void DeletePerson(PersonDTO person);
        decimal TotalBalanceOnAccounts(int id);
    }
}
