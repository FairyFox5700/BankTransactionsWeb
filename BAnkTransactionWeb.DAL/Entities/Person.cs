﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankTransactionWeb.DAL.Entities
{
    public class Person :BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(35)]
        public string LastName { get; set; }
        public DateTime DataOfBirth { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
