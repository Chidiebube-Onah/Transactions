using AutoMapper;
using Transactions.Model.Dtos.Response;

namespace Transactions.Model.Configuration.MappingConfiguration;

public class PagedResponseProfile : Profile
{
    public PagedResponseProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedResponse<>))
            .ConvertUsing(typeof(PagedListToPagedResponseConverter<,>));
    }


    public class PagedListToPagedResponseConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedResponse<TDestination>> where TDestination : class
    {
        public PagedResponse<TDestination> Convert(PagedList<TSource> source, PagedResponse<TDestination> destination, ResolutionContext context)
        {
            var list = source.ToList();
            var items = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);

            var pagedResponse = new PagedResponse<TDestination>
            {
                MetaData = source.MetaData,
                Items = items
            };
            return pagedResponse;
        }
    }
}