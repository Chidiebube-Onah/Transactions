using CryptoClient.Infrastructure.Model;
using CryptoClient.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace CryptoClient.Api.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class MockThirdPartyCryptoClient : ControllerBase
    {
        private readonly CryptoClientService _clientService;


        private readonly ILogger<MockThirdPartyCryptoClient> _logger;

        public MockThirdPartyCryptoClient(CryptoClientService clientService, ILogger<MockThirdPartyCryptoClient> logger)
        {
            _logger = logger;
            _clientService = clientService;
        }

        [HttpGet(Name = "GetTransaction")]
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
        public async Task<IActionResult> InitiateTransactions()
        {
            UpdateTransactionsCommandRequest? response = await _clientService.InitiateTransaction();

            return Ok(response);
        }

        [HttpGet("mock-update-commands",Name = "GetMockUpdateCommands")]
        public IActionResult GetMockUpdateCommands()
        {
            IEnumerable<UpdateTransactionsCommandRequest> response = _clientService.GetMockUpdateCommands();

            return Ok(response);
        }


        [HttpPost("toggle-availability", Name = "ToggleAvailability")]
        public IActionResult ToggleAvailability()
        {
            string message = _clientService.TriggerError();

            return Ok(message);
        }

    }
}