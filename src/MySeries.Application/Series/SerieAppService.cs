﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace MySeries.Series
{
    [Authorize]
    public class SerieAppService : CrudAppService<WatchList, SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto>, ISeriesAppService
    {
        private readonly ISeriesApiService _seriesService;
        public SerieAppService(IRepository<Serie, int> repository, ISeriesApiService seriesService) : base(repository)
        {
            _seriesService = seriesService;
        }

        public async Task<ICollection<SerieDto>> SearchAsync(string title, string gender)
        {
            return await _seriesService.GetSeriesAsync(title, gender);
            
        }
    }
}
