using BankTransaction.Configuration;
using BankTransaction.Models;
using BankTransaction.Web.Models;

namespace BankTransaction.Web.Mapper.Filters
{
    public class ShareholderSearchToFilterDto : IMapper<ShareholderSearchModel, ShareholderFilterModel>
    {
        private ShareholderSearchToFilterDto() { }

        public static readonly ShareholderSearchToFilterDto Instance = new ShareholderSearchToFilterDto();
        public ShareholderFilterModel Map(ShareholderSearchModel source)
        {
            return new ShareholderFilterModel()
            {
                CompanyName = source.CompanyName,
                DateOfCompanyCreation = source.DateOfCompanyCreation
            };
        }

        public ShareholderSearchModel MapBack(ShareholderFilterModel destination)
        {
            return new ShareholderSearchModel()
            {
                CompanyName = destination.CompanyName,
                DateOfCompanyCreation = destination.DateOfCompanyCreation
            };
        }
    }
}
