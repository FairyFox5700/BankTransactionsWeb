using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Configuration
{
    public class RedisCacheConfiguration
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
    }
}
