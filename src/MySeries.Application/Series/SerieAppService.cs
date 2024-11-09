using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Series
{
    [Authorize]
    public class SerieAppService : CrudAppService<Serie, SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto, CreateUpdateSerieDto>, ISeriesAppService
    {
        private readonly ISeriesApiService _seriesApiService;
        public SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesApiService) : base(repository)
        {
            _seriesApiService = seriesApiService;
        }

        public async Task<ICollection<SerieDto>> SearchAsync(string? title, string? gender)
        {
            return await _seriesApiService.GetSeriesAsync(title, gender);
        }
    }
}
