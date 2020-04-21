using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.Models
{
    public class PaginatedModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}

