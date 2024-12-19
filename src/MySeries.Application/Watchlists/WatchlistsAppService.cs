using MySeries.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Microsoft.AspNetCore.Authorization;
using MySeries.Api;

namespace MySeries.Watchlists
{
    [Authorize]
    public class WatchlistsAppService : ApplicationService, IWatchlistAppService
    {
        private readonly IRepository<Serie, int> _serieRepository;
        private readonly IRepository<Watchlist, int> _watchlistRepository;
        private readonly ICurrentUser _currentUser;
        private readonly OmdbApiService _omdbApiService;
        private readonly SerieAppService _serieAppService;

        public WatchlistsAppService(IRepository<Serie, int> serieRepository, IRepository<Watchlist, int> watchlistRepository, ICurrentUser currentUser)
        {
            _serieRepository = serieRepository;
            _watchlistRepository = watchlistRepository;
            _currentUser = currentUser;
        }

        public async Task AgregarSerieAsync(string title)
        {
            Guid? userID = _currentUser.Id;
            var watchlist = (await _watchlistRepository.GetListAsync()).FirstOrDefault();

            if (watchlist == null)
            {
                watchlist = new Watchlist();
                await _watchlistRepository.InsertAsync(watchlist);
            }

            var serieApi = await _omdbApiService.BuscarSerieAsync(title, null);

            if (!watchlist.SerieList.Any(s => s.IdSerie == serieApi.FirstOrDefault().IdSerie))
            {
                await _serieAppService.ObtenerSeriesAsync(serieApi);
                var serie = (await _serieRepository.GetListAsync()).FirstOrDefault();
                watchlist.SerieList.Add(serie);

            }
            else
            {
                throw new Exception("La Serie ya se encuentra");
            }

            await _watchlistRepository.UpdateAsync(watchlist);

        }
    }
}
