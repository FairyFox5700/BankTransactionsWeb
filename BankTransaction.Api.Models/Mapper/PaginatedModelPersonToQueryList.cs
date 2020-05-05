using BankTransaction.Api.Models.Queries;
using BankTransaction.BAL.Implementation.DTOModels;
using BankTransaction.Configuration;
using BankTransaction.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankTransaction.Api.Models.Mapper
{
    public class PaginatedModelPersonToQueryList : IMapper<PaginatedModel<PersonDTO>, PageQueryParameters>
    {
        private PaginatedModelPersonToQueryList() { }

        public static readonly PaginatedModelPersonToQueryList Instance = new PaginatedModelPersonToQueryList();
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
