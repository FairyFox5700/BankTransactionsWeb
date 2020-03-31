using BankTransactionWeb.BAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BankTransactionWeb.BAL.Interfaces
{
   public interface IPersonService
    {
        Task<List<PersonDTO>> GetAllPersons();
        Task<PersonDTO> GetPersonById(int id);
        Task AddPerson(PersonDTO person);
        Task UpdatePerson(PersonDTO person);
        Task DeletePerson(PersonDTO person);
        Task<decimal> TotalBalanceOnAccounts(int id);
    }
}
