using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTransaction.Api.Models
{
    public class PaginatedList<T> 
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        //public int TotalPages { get; private set; }
       // public string NextPage { get; set; }
       // public string PreviosPage { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
     
       // public bool HasPrevious => PageNumber > 1;
       // public bool HasNext => PageNumber < TotalPages;


        public PaginatedList(IEnumerable<T> items, int pageNumber, int pageSize)
        {
            Data = items;
            //var count = items.Count();
           //this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
        //public static PaginatedList<T> ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        //{
        //    var count = source.Count();
        //    var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        //    return new PaginatedList<T>(items, count, pageIndex, pageSize);
        //}


    }
}

