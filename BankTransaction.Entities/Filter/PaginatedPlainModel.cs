using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.Entities.Filter
{
    public class PaginatedPlainModel<T> : List<T>
    {

        const int MaxPageSize = 30;
        private int pageNumber { get; set; }
        private int pageSize;
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        //public bool HasPrevious => PageIndex < TotalPages;
        //public bool HasNext => PageIndex > 1;

        public PaginatedPlainModel(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }


        public int PageNumber
        {
            get => pageNumber;
            set
            {
                if (value >= 1)
                    pageNumber = value;
            }
        }
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (value >= 1)
                    pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }

        //public PaginatedPlainModel()
        //{
        //    this.pageNumber = 1;
        //    this.pageSize = 10;
        //}
        public static async Task<PaginatedPlainModel<T>> Paginate(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count =await source.CountAsync();
            var result = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            return new PaginatedPlainModel<T>(result, count, pageIndex, pageSize);
        }
       
    }
}
