using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoClient.Infrastructure.Extensions;
using CryptoClient.Infrastructure.Model;
using MassTransit;

namespace CryptoClient.Infrastructure.Services
{
    public  class CryptoClientService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IPublishEndpoint _publishEndpoint;

        public bool _triggerError { get; private set; }

        private static List<CryptoTransaction> _transactionsDataStore;

        private static List<UpdateTransactionsCommandRequest> _updateTransactionsCommands;
            

        private static readonly Random Random = new Random();


        private static readonly Dictionary<string, string> CurrencyNetworkMap = new Dictionary<string, string>
        {
            { "BTC", "BSC" },
            { "LTC", "ETHEREUM" },
            { "BNB", "POLYGON" },
            { "USDT", "CELO" },
            { "DAI", "TRON" },
            { "BUSD", "BSC" },
            { "XEND", "ETHEREUM" },
            { "ETH", "POLYGON" },
            { "MATIC", "CELO" },
            { "WNT", "TRON" },
            { "USDC", "BSC" },
            { "USD", "ETHEREUM" },
            { "cEUR", "POLYGON" },
            { "CELO", "CELO" },
            { "TRX", "TRON" },
            { "USDJ", "BSC" },
            { "TUSD", "ETHEREUM" }
        };


        private enum Currency
        {
            BTC = 1,
            LTC = 2,
            BNB = 3,
            USDT = 4,
            DAI = 5,
            BUSD = 6,
            XEND = 7,
            ETH = 8,
            MATIC = 9,
            WNT = 10,
            USDC = 11,
            USD = 12,
            cEUR = 13,
            CELO = 14,
            TRX = 15,
            USDJ = 16,
            TUSD = 17
        }

        private enum NetworkChain
        {
            BSC = 1,
            ETHEREUM = 2,
            POLYGON = 3,
            CELO = 4,
            TRON = 8
        }

        private enum TransactionType
        {
            Deposit,
            Transfer
        }

        public CryptoClientService( IMessageQueue messageQueue,  IPublishEndpoint publishEndpoint)
        {
            _messageQueue = messageQueue;
            _publishEndpoint = publishEndpoint;
            _transactionsDataStore = GenerateRandomTransactions(1000);
            _updateTransactionsCommands =
                GenerateRandomUpdateTransactionsCommands(_transactionsDataStore, 6);

        }

        public  CryptoTransaction? GetTransaction(CryptoClientRequest request)
        {
            CryptoTransaction transaction = _transactionsDataStore.FirstOrDefault(t =>
                t.TransactionHash.Equals(request.TransactionHash) && t.FromAddress.Equals(request.WalletAddress) &&
                t.Currency.Equals(request.Currency));

            return transaction;
        }

        public async  Task<UpdateTransactionsCommandRequest> InitiateTransaction()
        {
            string walletAddress = _transactionsDataStore[Random.Next(_transactionsDataStore.Count)].FromAddress;
            CryptoTransaction randomTransaction = GenerateRandomTransaction(walletAddress);
           
            if (IsWalletAddressExists(_transactionsDataStore, walletAddress))
            {
                randomTransaction = GenerateRandomTransaction(walletAddress);
                _transactionsDataStore.Add(randomTransaction);
            }

            if (_transactionsDataStore.LastOrDefault().TransactionHash.Equals(randomTransaction.TransactionHash))
            {
                UpdateTransactionsCommandRequest command = new UpdateTransactionsCommandRequest
                {
                    ClientId = Guid.NewGuid().ToString(),
                    TransactionHash = randomTransaction.TransactionHash,
                    Currency = randomTransaction.Currency,
                    WalletAddress = walletAddress
                };

                // await _publishEndpoint.Publish<UpdateTransaction>(new
                // {
                //     ClientId = Guid.NewGuid(),
                //     randomTransaction.TransactionHash,
                //     randomTransaction.Currency,
                //     WalletAddress = walletAddress,
                //     Created = DateTime.UtcNow
                // });

                await _messageQueue.PublishAsync(command, "UpdateTransactions");

                return command;
            }

            return null;
        }

        public  IEnumerable<UpdateTransactionsCommandRequest> GetMockUpdateCommands()
        {
            return _updateTransactionsCommands;
        }

        public string TriggerError()
        {
            _triggerError = !_triggerError;

            string message = _triggerError ? "Not Available" : "Available";
            return message;
        }
        private static List<CryptoTransaction> GenerateRandomTransactions(int count)
        {
            List<CryptoTransaction> transactions = new List<CryptoTransaction>();

            for (int i = 0; i < count; i++)
            {
                string transactionType = GetRandomTransactionType();
                string toAddress = GenerateRandomHexString(40);
                string currency = GetRandomCurrency();
                string network = GetNetworkChainForCurrency(currency);
                string transactionHash = GenerateRandomHexString(64);
                string fromAddress = GenerateRandomHexString(40);
                DateTime createdAt = GenerateRandomDateTime();
                string transactionStatus = GetRandomTransactionStatus();

                CryptoTransaction transaction = new CryptoTransaction
                {
                    TransactionStatus = transactionStatus,
                    TransactionType = transactionType,
                    TransactionHash = transactionHash,
                    TransactionHashUrl = $"https://testnet.bscscan.com/tx/{transactionHash}",
                    ToAddress = toAddress,
                    FromAddress = fromAddress,
                    ToAddressUrl = $"https://testnet.bscscan.com/address/{toAddress}",
                    FromAddressUrl = $"https://testnet.bscscan.com/address/{fromAddress}",
                    Amount = GenerateRandomAmount(),
                    Network = network,
                    Currency = currency,
                    TransactionTimeStamp = createdAt,
                    Reference = $"XF-WREF-{GenerateRandomHexString(24)}",
                    CreatedAt = createdAt
                };

                transactions.Add(transaction);
            }

            return transactions;
        }

        private static bool IsWalletAddressExists(List<CryptoTransaction> transactions, string walletAddress)
        {
            foreach (CryptoTransaction transaction in transactions)
            {
                if (transaction.FromAddress == walletAddress)
                {
                    return true;
                }
            }

            return false;
        }

        private static CryptoTransaction GenerateRandomTransaction(string walletAddress)
        {
            string transactionType = GetRandomTransactionType();
            string toAddress = GenerateRandomHexString(40);
            string currency = GetRandomCurrency();
            string network = GetNetworkChainForCurrency(currency);
            string transactionHash = GenerateRandomHexString(64);
            string fromAddress = GenerateRandomHexString(40);
            DateTime createdAt = GenerateRandomDateTime();

            string transactionStatus = GetRandomTransactionStatus();

            CryptoTransaction transaction = new CryptoTransaction
            {
                TransactionStatus = transactionStatus,
                TransactionType = transactionType,
                TransactionHash = transactionHash,
                TransactionHashUrl = $"https://testnet.bscscan.com/tx/{transactionHash}",
                ToAddress = toAddress,
                FromAddress = walletAddress,
                ToAddressUrl = $"https://testnet.bscscan.com/address/{toAddress}",
                FromAddressUrl = $"https://testnet.bscscan.com/address/{walletAddress}",
                Amount = GenerateRandomAmount(),
                Network = network,
                Currency = currency,
                TransactionTimeStamp = createdAt,
                Reference = $"XF-WREF-{GenerateRandomHexString(24)}",
                CreatedAt = createdAt
            };

            return transaction;
        }

        private static string GetRandomTransactionType()
        {
            Array values = Enum.GetValues(typeof(TransactionType));
            return ((TransactionType)(values.GetValue(Random.Next(values.Length)) ?? string.Empty)).ToString();
        }

        private static string GetRandomCurrency()
        {
            Array values = Enum.GetValues(typeof(Currency));
            return ((Currency)values.GetValue(Random.Next(values.Length))).ToString();
        }

        private static string GetNetworkChainForCurrency(string currency)
        {
            if (CurrencyNetworkMap.TryGetValue(currency, out var value))
            {
                return value.ToString();
            }

            return NetworkChain.BSC.ToString(); // Default to BSC if mapping is not found
        }

        private static string GenerateRandomHexString(int length)
        {
            const string chars = "0123456789ABCDEF";
            char[] hexArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                hexArray[i] = chars[Random.Next(chars.Length)];
            }
            return new string(hexArray);
        }

        private static decimal GenerateRandomAmount()
        {
            double amount = Random.NextDouble() * 1000; // Generate a random amount between 0 and 1000
            decimal roundedAmount = decimal.Round((decimal)amount, 2); // Round the amount to 2 decimal places
            return roundedAmount;
        }

        private static DateTime GenerateRandomDateTime()
        {
            DateTime startDateTime = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - startDateTime).Days;
            return startDateTime.AddDays(Random.Next(range)).AddHours(Random.Next(24)).AddMinutes(Random.Next(60)).AddSeconds(Random.Next(60)).ToUniversalTime();
        }

        private static string GetTransactionsAsJson(List<CryptoTransaction> transactions)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(transactions, Newtonsoft.Json.Formatting.Indented);
        }

        private static string GetRandomTransactionStatus()
        {
            // Define a list of possible transaction statuses
            List<string> transactionStatuses = new List<string>
        {
            "SUCCESSFUL",
            "PENDING",
            "FAILED"
        };

            // Randomly select a transaction status from the list
            return transactionStatuses[Random.Next(transactionStatuses.Count)];
        }

        private static List<UpdateTransactionsCommandRequest> GenerateRandomUpdateTransactionsCommands(List<CryptoTransaction> transactions, int numberOfCommands)
        {
            List<UpdateTransactionsCommandRequest> commands = new List<UpdateTransactionsCommandRequest>();

            for (int i = 0; i < numberOfCommands; i++)
            {
                // Generate a random ClientId
                Guid clientId = Guid.NewGuid();

                // Randomly select a transaction from the list
                CryptoTransaction selectedTransaction = transactions[Random.Next(transactions.Count)];

                // Create an UpdateTransactionsCommandDto using the selected transaction's properties
                UpdateTransactionsCommandRequest command = new UpdateTransactionsCommandRequest
                {
                    ClientId = clientId.ToString(),
                    WalletAddress = selectedTransaction.FromAddress,
                    Currency = selectedTransaction.Currency,
                    TransactionHash = selectedTransaction.TransactionHash
                };

                commands.Add(command);

                // Generate additional commands for the same ClientId with different wallet addresses and transaction hashes
                if (i % 2 == 0)
                {
                    // Randomly select another transaction from the list
                    CryptoTransaction additionalTransaction = transactions[Random.Next(transactions.Count)];

                    // Create an additional UpdateTransactionsCommandDto using the additional transaction's properties
                    UpdateTransactionsCommandRequest additionalCommand = new UpdateTransactionsCommandRequest
                    {
                        ClientId = clientId.ToString(),
                        WalletAddress = additionalTransaction.FromAddress,
                        Currency = additionalTransaction.Currency,
                        TransactionHash = additionalTransaction.TransactionHash
                    };

                    commands.Add(additionalCommand);
                }
            }

            return commands;
        }

    }
}
