using CryptoClient.Infrastructure.Model;
using CryptoClient.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CryptoClient.Api.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class MockThirdPartyCryptoClient : ControllerBase
    {
        private readonly CryptoClientService _clientService;
        private readonly ITransactionService _transactionService;


        public MockThirdPartyCryptoClient(CryptoClientService clientService, ITransactionService transactionService)
        {
            _clientService = clientService;
            _transactionService = transactionService;
        }

        [HttpGet(Name = "GetTransaction")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "To Fetch a transaction, this simulates the third-party crypto api service", Type = typeof(CryptoTransaction))]
       
        public IActionResult GetTransactions([FromQuery]CryptoClientRequest request)
        {
            if (_clientService._triggerError)
            {
                return StatusCode(503, "Service unavailable");
            }

            CryptoTransaction? response = _clientService.GetTransaction(request);

            return Ok(response);
        }


        
        [HttpPost("initiate-transaction" ,Name = "InitiateTransaction")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "To Update a transaction , this UpdateTransactions command, that is sent to the Transactions Micro-service", Type = typeof(CryptoTransaction))]
        public async Task<IActionResult> InitiateTransactions()
        {
            UpdateTransactionsCommandRequest? response = await _transactionService.Update();

            return Ok(response);
        }

        [HttpGet("mock-update-commands",Name = "GetMockUpdateCommands")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Generates Random Update Request, for debug purpose, Nb, this doesn't send any message to the queue", Type = typeof(CryptoTransaction))]

        public IActionResult GetMockUpdateCommands()
        {
            IEnumerable<UpdateTransactionsCommandRequest> response = _clientService.GetMockUpdateCommands();

            return Ok(response);
        }


        [SwaggerResponse(StatusCodes.Status200OK, Description = "Toggles the availability of  the third-party crypto api service", Type = typeof(string))]
        [HttpPost("toggle-availability", Name = "ToggleAvailability")]
        public IActionResult ToggleAvailability()
        {
            string message = _clientService.TriggerError();

            return Ok(message);
        }

    }
}