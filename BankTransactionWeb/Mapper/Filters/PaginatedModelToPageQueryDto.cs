using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Models;
using PageQueryParameters = BankTransaction.Web.Models.PageQueryParameters;

namespace BankTransaction.Web.Mapper.Filters
{
    public class PaginatedModelToPageQueryDto : IMapper<PaginatedModel<PersonDTO>, PageQueryParameters>
    {
        private PaginatedModelToPageQueryDto() { }

        public static readonly PaginatedModelToPageQueryDto Instance = new PaginatedModelToPageQueryDto();
        public PageQueryParameters Map(PaginatedModel<PersonDTO> source)
        {
            return new PageQueryParameters(startIndex: 0, count: source.Count)
            {
                PageNumber = source.PageNumber,
                PageSize = source.PageSize
            };
        }

        public PaginatedModel<PersonDTO> MapBack(PageQueryParameters destination)
        {
            return new PaginatedModel<PersonDTO>(items: null, pageNumber: destination.PageNumber, pageSize: destination.PageSize, totalCount: 0, totalPages: 0);
        }
    }
}
