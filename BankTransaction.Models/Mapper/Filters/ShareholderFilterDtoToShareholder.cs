using BankTransaction.Configuration;
using BankTransaction.Entities.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Models.Mapper.Filters
{
    public class ShareholderFilterDtoToShareholder : IMapper<ShareholderFilterModel, ShareholderFilter>
    {
        private ShareholderFilterDtoToShareholder() { }
        public static readonly ShareholderFilterDtoToShareholder Instance = new ShareholderFilterDtoToShareholder();
        public ShareholderFilter Map(ShareholderFilterModel source)
        {
            return new ShareholderFilter()
            {
                CompanyName = source.CompanyName,
                DateOfCompanyCreation = source.DateOfCompanyCreation
            };
        }

        public ShareholderFilterModel MapBack(ShareholderFilter destination)
        {
            return new ShareholderFilterModel()
            {
                CompanyName = destination.CompanyName,
                DateOfCompanyCreation = destination.DateOfCompanyCreation
            };
        }
    }
}
