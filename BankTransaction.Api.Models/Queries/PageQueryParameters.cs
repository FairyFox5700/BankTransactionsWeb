namespace BankTransaction.Api.Models.Queries
{
    public class PageQueryParameters
    {
        const int MaxPageSize = 30;
        private int pageNumber { get; set; }
        public int PageNumber {
            get=>pageNumber;
            set
            {
                if (value >= 1)
                    pageNumber = value;
            }
        }


        private int pageSize;
        public int PageSize 
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value >= 1)
                    pageSize = (value  > MaxPageSize) ? MaxPageSize : value;
            }
        }

        public PageQueryParameters()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public PageQueryParameters(int startIndex, int count)
        {
            PageNumber = startIndex;
            PageSize = count;
        }
    }
}
