using BankTransaction.Api.Helpers;
using BankTransaction.Api.Models;
using BankTransaction.Api.Models.Mapper;
using BankTransaction.Api.Models.Queries;
using BankTransaction.Api.Models.Responces;
using BankTransaction.BAL.Abstract;
using BankTransaction.BAL.Implementation.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankTransaction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareholderController : ControllerBase
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
        [Cached(2000)]
        public async Task<ApiResponse<PaginatedList<ShareholderDTO>>> GetAllShareholders([FromQuery]PageQueryParameters pageQueryParameters, [FromQuery]SearchShareholderQuery searchShareholderQuery)
        {
            var paginatedModel = PaginatedModelShareholderToQueryList.Instance.MapBack(pageQueryParameters);
            var filter = ShareholderFilterToSearchMapper.Instance.MapBack(searchShareholderQuery);
            var shareholders = await shareholderService.GetAllShareholders(pageQueryParameters.PageNumber, pageQueryParameters.PageSize, filter);
            var paginatedShareholders = new PaginatedList<ShareholderDTO>(shareholders, paginatedModel);
            return new ApiResponse<PaginatedList<ShareholderDTO>>(paginatedShareholders);
        }
        // PUT /api/Shareholder/{id}
        [HttpPut("{id}")]
        public async Task<ApiResponse<int>> UpdateShareholder(int id, ShareholderDTO shareholder)
        {
            if (id != shareholder.Id)
            {
                return ApiResponse<int>.BadRequest;
            }
            var currentShareholder = await shareholderService.GetShareholderById(id);
            if (currentShareholder == null)
            {
                return ApiResponse<int>.NotFound;
            }
            var result = await shareholderService.UpdateShareholder(shareholder);
            if (result.IsError)
                return new ApiResponse<int>(400, new ApiErrorResponse() { Message = result.Message });
            return new ApiResponse<int>(id);

        }

        // POST: api/Shareholder
        [HttpPost]
        public async Task<ApiResponse<ShareholderDTO>> AddShareholder(ShareholderDTO shareholder)
        {
           var result= await shareholderService.AddShareholder(shareholder);
            if (result.IsError)
                return new ApiResponse<ShareholderDTO>(400, new ApiErrorResponse() { Message = result.Message });
            return new ApiResponse<ShareholderDTO>(shareholder);
        }
        // DELETE /api/Shareholder/{id}
        [HttpDelete("{id}")]
        public async Task<ApiResponse<ShareholderDTO>> DeleteShareholder(int id)
        {
            var shareholder = await shareholderService.GetShareholderById(id);
            if (shareholder == null)
            {
                return ApiResponse<ShareholderDTO>.NotFound;
            }
            await shareholderService.DeleteShareholder(shareholder);
            return new ApiResponse<ShareholderDTO>(shareholder);

        }
    }
}