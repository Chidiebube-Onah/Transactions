using AutoMapper;
using CryptoClient.Infrastructure.Model;
using Transactions.Model.Dtos.Request;
using Transactions.Model.Entities;

namespace Transactions.Model.Configuration.MappingConfiguration;

public class TransactionUpdateRequestProfile : Profile
{
    public TransactionUpdateRequestProfile()
    {
        CreateMap<UpdateTransactionsCommandRequest, TransactionUpdateRequest>();
    }
}