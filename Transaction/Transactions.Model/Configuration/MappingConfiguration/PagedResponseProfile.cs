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
            List<TSource> list = source.ToList();
            IEnumerable<TDestination>? items = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);

            PagedResponse<TDestination> pagedResponse = new PagedResponse<TDestination>
            {
                MetaData = source.MetaData,
                Items = items
            };
            return pagedResponse;
        }
    }
}