using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.DTOModels
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
