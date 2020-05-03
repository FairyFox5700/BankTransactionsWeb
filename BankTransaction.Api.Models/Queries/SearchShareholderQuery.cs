using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models.Queries
{
    public class SearchShareholderQuery
    {
        public string CompanyName { get; set; }
        public DateTime DateOfCompanyCreation { get; set; }
    }
}
