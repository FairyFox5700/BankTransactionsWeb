using BankTransaction.Api.Models.Queries;
using BankTransaction.Configuration;
using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models.Mapper
{
    public class ShareholderFilterToSearchMapper : IMapper<ShareholderFilterModel, SearchShareholderQuery>
    {
        private ShareholderFilterToSearchMapper() { }

        public static readonly ShareholderFilterToSearchMapper Instance = new ShareholderFilterToSearchMapper();
        public SearchShareholderQuery Map(ShareholderFilterModel source)
        {
            return new SearchShareholderQuery()
            {
                CompanyName = source.CompanyName,
                DateOfCompanyCreation = source.DateOfCompanyCreation
            };
        }

        public ShareholderFilterModel MapBack(SearchShareholderQuery destination)
        {
            return new ShareholderFilterModel()
            {
                CompanyName = destination.CompanyName,
                DateOfCompanyCreation = destination.DateOfCompanyCreation
            };
        }
    }
}
