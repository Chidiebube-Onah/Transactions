using AutoMapper;
using Transactions.Model.Dtos.Response;
using Transactions.Model.Entities;

namespace Transactions.Model.Configuration.MappingConfiguration;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionResponse>().ReverseMap();
    }

}