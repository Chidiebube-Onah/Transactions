using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transactions.Model.Dtos.Request;
using Transactions.Model.Dtos.Response;
using Transactions.Services.Infrastructure;
using Transactions.Services.Interfaces;

namespace Transactions.Api.Controllers
{
    [ApiController]
    [Route("transactions")]
    
    public class TransactionsController : BaseController
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService )
        {
            _transactionsService = transactionsService;
        }

        [HttpGet(Name = "GetTransactions")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Successful", Type = typeof(PagedResponse<TransactionResponse>))]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionRequestParams request)
        {
            PagedResponse<TransactionResponse> response = await _transactionsService.GetTransactions(request);
            return Ok(response);
        }
    }
}