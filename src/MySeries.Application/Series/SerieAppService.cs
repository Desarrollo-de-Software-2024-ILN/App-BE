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
        private readonly ISeriesService _seriesService;
        public SerieAppService(IRepository<Serie, int> repository, ISeriesService seriesService) : base(repository)
        {
            _seriesService = seriesService;
        }

        public async Task<SerieDto> SearchAsync(string title, string gender)
        {
            return await _seriesService.GetSeriesAsync(title, gender);
            
        }
    }
}
