using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Web.Models
{
    public class ShareholderSearchModel
    {
        public string CompanyName { get; set; }
        public DateTime? DateOfCompanyCreation { get; set; }
    }
}
