namespace BankTransaction.Api.Models.Query
{
    public class PageQueryParameters
    {
        const int MaxPageSize = 30;
        private int startIndex { get; set; }
        public int StartIndex {
            get=>startIndex;
            set
            {
                if (value >= 1)
                    startIndex = value;
            }
        }


        private int count;
        public int Count 
        {
            get
            {
                return count;
            }
            set
            {
                if (value >= 1)
                    count = (value  > MaxPageSize) ? MaxPageSize : value;
            }
        }

        public PageQueryParameters()
        {
            this.StartIndex = 1;
            this.Count = 10;
        }

        public PageQueryParameters(int startIndex, int count)
        {
            StartIndex = startIndex;
            Count = count;
        }
    }
}
