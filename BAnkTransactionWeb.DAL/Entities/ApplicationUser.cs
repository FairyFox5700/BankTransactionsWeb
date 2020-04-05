using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Person Person { get; set; }
    }
}
