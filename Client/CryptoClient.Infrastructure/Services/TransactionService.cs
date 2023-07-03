using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoClient.Infrastructure.Model;

namespace CryptoClient.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly CryptoClientService _clientService;
        private readonly IMessageQueue _messageQueue;

        public TransactionService( CryptoClientService clientService, IMessageQueue messageQueue )
        {
            _clientService = clientService;
            _messageQueue = messageQueue;
        }

        public async Task<UpdateTransactionsCommandRequest?> Update()
        {
            UpdateTransactionsCommandRequest command = _clientService.InitiateTransaction();

            await _messageQueue.PublishAsync(command, "UpdateTransactions");

            return command;
        }
    }
}
