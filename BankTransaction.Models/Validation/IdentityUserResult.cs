using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace BankTransaction.Models.Validation
{
    public class IdentityUserResult
    {
        public static readonly IdentityUserResult SUCCESS = new IdentityUserResult { Succeeded = true };
        public static readonly IdentityUserResult LOCKED = new IdentityUserResult { Locked = true };
        public bool Succeeded { get; set; } = false;
        public bool NotFound{ get; set; } = false;
        public bool Locked { get; set; } = false;
        public IEnumerable<string> Errors { get; set; }
        public static IdentityUserResult GenerateErrorResponce(IdentityResult result)
        {
            return new IdentityUserResult { Errors = new List<string>( result.Errors.Select(x => x.Description))};
        }
    }
}
