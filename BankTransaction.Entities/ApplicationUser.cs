
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Person Person { get; set; }
    }
}
