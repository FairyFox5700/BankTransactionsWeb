using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankTransaction.Models
{
    public class PaginatedModel<T>:List<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
        
        public PaginatedModel()
        {
                
        }
        public PaginatedModel(IEnumerable<T>items, int pageNumber, int pageSize, int totalCount, int totalPages)
        {
            AddRange(items);
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }

    }
}

