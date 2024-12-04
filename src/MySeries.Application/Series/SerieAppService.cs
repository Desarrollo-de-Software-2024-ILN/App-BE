using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Series
{
    public class SerieAppService : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto>, ISeriesAppService
    {
        private readonly ISeriesApiService _seriesApiService;

        public SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesApiService) : base(repository)
        {
            _seriesApiService = seriesApiService;
        }

        public async Task<SerieDto[]> BuscarSerieAsync(string Title, string Genre = null)
        {
            return await _seriesApiService.BuscarSerieAsync(Title, Genre);
        }
    }
}
