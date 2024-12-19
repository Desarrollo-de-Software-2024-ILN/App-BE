using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using System.Collections.Generic;
using Volo.Abp.Users;
using System;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Authorization;
using MySeries.Series;
using MySeries.Notifications;

namespace MySeries.Updates
{
    [Authorize]
    public class SerieUpdateService : DomainService, ISerieUpdateChecker
    {
        private readonly IRepository<Serie, int> _repository;
        private readonly ISeriesApiService _seriesApiService;
        private readonly INotificationService _notificationService;
        private readonly IObjectMapper _ObjectMapper;

        public SerieUpdateService (IRepository<Serie, int> repository, ISeriesApiService seriesApiService, INotificationService notificationService, IObjectMapper objectMapper)
        {
            _repository = repository;
            _seriesApiService = seriesApiService;
            _notificationService = notificationService;
            _ObjectMapper = objectMapper;
        }

        public async Task UpdateSeriesAsync()
        {
            var series = await _repository.GetListAsync();
            foreach (var serie in series)
            {
                var seriesApi = await _seriesApiService.BuscarSerieAsync(serie.Title, serie.Genre);

                if (seriesApi != null & seriesApi.Length > 0)
                {
                    var serieApi = seriesApi.FirstOrDefault();

                }
            }
        }
    }
}
