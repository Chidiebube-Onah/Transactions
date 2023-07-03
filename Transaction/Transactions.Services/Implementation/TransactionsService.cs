using AutoMapper;
using CryptoClient.Infrastructure.Model;
using Transactions.Data.Interfaces;
using Transactions.Model.Dtos.Request;
using Transactions.Model.Dtos.Response;
using Transactions.Model.Entities;
using Transactions.Model.Enums;
using Transactions.Services.Interfaces;

namespace Transactions.Services.Implementation;

public class TransactionsService : ITransactionsService
{
    private readonly ICryptoApiClient _cryptoApiClient;
    private readonly IMapper _mapper;
    private readonly IMessageQueue _messageQueue;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Transaction> _transactionsRepository;
    private readonly IRepository<TransactionUpdateRequest> _transactionsUpdateRequestRepository;

    public TransactionsService(IUnitOfWork unitOfWork, IMapper mapper, ICryptoApiClient cryptoApiClient,
        IMessageQueue messageQueue)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _messageQueue = messageQueue;
        _cryptoApiClient = cryptoApiClient;
        _transactionsRepository = _unitOfWork.GetRepository<Transaction>();
        _transactionsUpdateRequestRepository = _unitOfWork.GetRepository<TransactionUpdateRequest>();
    }

    public async Task<PagedResponse<TransactionResponse>> GetTransactions(TransactionRequestParams request)
    {
        var transactions = !string.IsNullOrWhiteSpace(request.SearchTerm)

            ? await _transactionsRepository.GetPagedItems(request,
                t => (string.IsNullOrEmpty(request.Currency) || t.Currency == request.Currency)
                     && (string.IsNullOrEmpty(request.Network) || t.Network == request.Network)
                     && (string.IsNullOrEmpty(request.Status) || t.TransactionStatus == request.Status)
                     && (string.IsNullOrEmpty(request.WalletAddress) || t.FromAddress == request.WalletAddress)
                     && (string.IsNullOrEmpty(request.TransactionType) || t.TransactionType == request.TransactionType)
                     && (request.StartTimeStamp == null || t.TransactionTimeStamp >= request.StartTimeStamp)
                     && (request.EndTimeStamp == null || t.TransactionTimeStamp <= request.EndTimeStamp)
                     && t.TransactionHash.ToLower().Contains(request.TransactionHash.ToLower()))

            : await _transactionsRepository.GetPagedItems(request,
                t => (string.IsNullOrEmpty(request.Currency) || t.Currency == request.Currency)
                     && (string.IsNullOrEmpty(request.Network) || t.Network == request.Network)
                     && (string.IsNullOrEmpty(request.Status) || t.TransactionStatus == request.Status)
                     && (string.IsNullOrEmpty(request.WalletAddress) || t.FromAddress == request.WalletAddress)
                     && (string.IsNullOrEmpty(request.TransactionType) || t.TransactionType == request.TransactionType)
                     && (request.StartTimeStamp == null || t.TransactionTimeStamp >= request.StartTimeStamp)
                     && (request.EndTimeStamp == null || t.TransactionTimeStamp <= request.EndTimeStamp));

        var response = _mapper.Map<PagedResponse<TransactionResponse>>(transactions);
        return response;
    }


    public async Task UpdateTransaction(UpdateTransactionsCommandRequest request)
    {
        if (await IsNewOrFailedRequest(request))
        {
            (var newTransaction, var isSuccessful) = await _cryptoApiClient.GetTransactions(request.TransactionHash,
                currencyType: request.Currency, walletAddress: request.WalletAddress);

            if (!isSuccessful) await LogNewTransactionUpdateRequest(request, RequestStatus.Failed);

            var isDuplicateTransaction = await TransactionExists(new UpdateTransactionsCommandRequest
            {
                TransactionHash = newTransaction?.TransactionHash,
                Currency = newTransaction?.Currency,
                WalletAddress = newTransaction?.FromAddress
            });

            if (!isDuplicateTransaction)
            {
                var transactionToAdd = _mapper.Map<Transaction>(newTransaction);

                await _transactionsRepository.AddAsync(transactionToAdd);

                await LogNewTransactionUpdateRequest(request, RequestStatus.Successful);

                await _messageQueue.PublishAsync(newTransaction, "NewTransactionCommand");
            }
        }
    }

    private async Task<bool> IsNewOrFailedRequest(UpdateTransactionsCommandRequest request)
    {
        var transactionUpdateRequest = await _transactionsUpdateRequestRepository.GetSingleByAsync(r =>
            r.TransactionHash.ToLower() == request.TransactionHash.ToLower() &&
            r.ClientId.ToLower() == request.ClientId.ToLower() &&
            r.WalletAddress.ToLower() == request.WalletAddress.ToLower() &&
            r.Currency.ToLower() == request.Currency.ToLower());

        if (transactionUpdateRequest?.Status == RequestStatus.Failed) return true;

        return false;
    }

    private async Task<bool> TransactionExists(UpdateTransactionsCommandRequest request)
    {
        var transaction = await _transactionsRepository.GetSingleByAsync(r =>
            r.TransactionHash.ToLower() == request.TransactionHash.ToLower() &&
            r.FromAddress.ToLower() == request.WalletAddress.ToLower() &&
            r.Currency.ToLower() == request.Currency.ToLower());

        return transaction != null;
    }

    private async Task LogNewTransactionUpdateRequest(UpdateTransactionsCommandRequest request, RequestStatus status)
    {
        var transactionUpdateRequest = _mapper.Map<TransactionUpdateRequest>(request,
            o => o.AfterMap((src, dest) => { dest.Status = status; }));

        await _transactionsUpdateRequestRepository.AddAsync(transactionUpdateRequest);
    }
}