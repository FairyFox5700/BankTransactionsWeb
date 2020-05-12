using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Mapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BankTransaction.Models.DTOModels;

namespace BankTransaction.Api.Controllers
{
    public class ShareholderController : BaseApiController
    {
        private readonly IShareholderService shareholderService;
        private readonly ILogger<ShareholderController> logger;

        public ShareholderController(IShareholderService shareholderService, ILogger<ShareholderController> logger)
        {
            this.shareholderService = shareholderService;
            this.logger = logger;
        }
        // GET /api/Shareholder
        [HttpGet]
        //[Cached(2000)]
        public async Task<ApiDataResponse<PaginatedList<ShareholderDTO>>> GetAllShareholders([FromQuery]PageQueryParameters pageQueryParameters, [FromQuery]SearchShareholderQuery searchShareholderQuery)
        {
            var paginatedModel = PaginatedModelShareholderToQueryList.Instance.MapBack(pageQueryParameters);
            var filter = ShareholderFilterToSearchMapper.Instance.MapBack(searchShareholderQuery);
            var shareholders = await shareholderService.GetAllShareholders(pageQueryParameters.PageNumber, pageQueryParameters.PageSize, filter);
            var paginatedShareholders = new PaginatedList<ShareholderDTO>(shareholders);
            return new ApiDataResponse<PaginatedList<ShareholderDTO>>(paginatedShareholders);
        }
        // PUT /api/Shareholder/{id}
        [HttpPut("{id}")]
        public async Task<ApiDataResponse<int>> UpdateShareholder(int id, ShareholderDTO shareholder)
        {
            if (id != shareholder.Id)
            {
                return ApiDataResponse<int>.BadRequest;
            }
            var currentShareholder = await shareholderService.GetShareholderById(id);
            if (currentShareholder == null)
            {
                return ApiDataResponse<int>.NotFound;
            }
            var result = await shareholderService.UpdateShareholder(shareholder);
            if (result.IsError)
                return new ApiDataResponse<int>(400, new ApiErrorResponse() { Message = result.Message });
            return new ApiDataResponse<int>(id);

        }

        // POST: api/Shareholder
        [HttpPost]
        public async Task<ApiDataResponse<ShareholderDTO>> AddShareholder(ShareholderDTO shareholder)
        {
           var result= await shareholderService.AddShareholder(shareholder);
            if (result.IsError)
                return new ApiDataResponse<ShareholderDTO>(400, new ApiErrorResponse() { Message = result.Message });
            return new ApiDataResponse<ShareholderDTO>(shareholder);
        }
        // DELETE /api/Shareholder/{id}
        [HttpDelete("{id}")]
        public async Task<ApiDataResponse<ShareholderDTO>> DeleteShareholder(int id)
        {
            var shareholder = await shareholderService.GetShareholderById(id);
            if (shareholder == null)
            {
                return ApiDataResponse<ShareholderDTO>.NotFound;
            }
            await shareholderService.DeleteShareholder(shareholder);
            return new ApiDataResponse<ShareholderDTO>(shareholder);

        }
    }
}