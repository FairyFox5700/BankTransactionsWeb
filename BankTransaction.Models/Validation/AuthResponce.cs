using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Validation
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
