using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransactionWeb.BAL.Models
{
    public sealed class AddPersonResponse : BaseGatewayResponse
    {
        public string Id { get; }
        public AddPersonResponse(string id, bool success = false, List<Error> errors = null) : base(success, errors)
        {
            Id = id;
        }
        
    }
}
