using AutoMapper;
using MySeries.Series;

namespace MySeries;

public class MySeriesApplicationAutoMapperProfile : Profile
{
    public MySeriesApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Series.WatchList, SerieDto>();
        CreateMap<CreateUpdateSerieDto, Series.WatchList>();
    }
}
