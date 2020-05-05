using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models.Mapper
{
    public class PaginatedModelShareholderToQueryList : IMapper<PaginatedModel<ShareholderDTO>, PageQueryParameters>
    {
        private PaginatedModelShareholderToQueryList() { }

        public static readonly PaginatedModelShareholderToQueryList Instance = new PaginatedModelShareholderToQueryList();
        public PageQueryParameters Map(PaginatedModel<ShareholderDTO> source)
        {
            return new PageQueryParameters(startIndex: 0, count: source.Count)
            {
                PageNumber = source.PageNumber,
                PageSize = source.PageSize
            };
        }

        public PaginatedModel<ShareholderDTO> MapBack(PageQueryParameters destination)
        {
            return new PaginatedModel<ShareholderDTO>(items: null, pageNumber: destination.PageNumber, pageSize: destination.PageSize, totalCount: 0, totalPages: 0);
        }
    }
}
