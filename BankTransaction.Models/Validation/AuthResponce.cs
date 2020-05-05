using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Validation
{
    public class AuthResult
    {//jwt
        
        public DateTime ExpieryDate { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public string RefreshToken { get; set; }
        public string MessageType { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string GetErrors => JsonConvert.SerializeObject(Errors);
    }
}
