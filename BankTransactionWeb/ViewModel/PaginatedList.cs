using System.Collections.Generic;
using BankTransaction.Models;

namespace BankTransaction.Web.ViewModel
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        public PaginatedList()
        {

        }
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public PaginatedList(PaginatedModel<T> paginatedModel)
        {
            Data = paginatedModel;
            if (paginatedModel != null)
            {
                TotalCount = paginatedModel.TotalCount;
                HasPrevious = paginatedModel.HasPrevious;
                HasNext = paginatedModel.HasNext;
                TotalPages = paginatedModel.TotalPages;
                PageNumber = paginatedModel.PageNumber;
                PageSize = paginatedModel.PageSize;
            }
        }
    }
}
