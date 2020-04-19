using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankTransaction.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string JwtId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpieryDate { get; set; }
        public string Token { get; set; }
        public bool IsUsed { get; set; }
        public bool IsInvalidated { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public ApplicationUser User {get;set;}
    }
}
