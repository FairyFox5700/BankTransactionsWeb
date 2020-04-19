using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankTransaction.Api.Models
{
    public class AuthSuccesfullModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
             
    }
}
