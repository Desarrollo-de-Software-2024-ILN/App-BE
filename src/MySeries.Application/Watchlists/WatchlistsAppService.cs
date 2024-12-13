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

namespace MySeries.Watchlists
{
    public class WatchlistsAppService : ApplicationService, IWatchlistAppService
    {
        private readonly IRepository<Serie, int> _serieRepository;
        private readonly IRepository<Watchlist, int> _watchlistRepository;
        private readonly ICurrentUser _currentUser;

        public WatchlistsAppService(IRepository<Serie, int> serieRepository, IRepository<Watchlist, int> watchlistRepository, ICurrentUser currentUser)
        {
            _serieRepository = serieRepository;
            _watchlistRepository = watchlistRepository;
            _currentUser = currentUser;
        }

        public async Task AgregarSerieAsync(int serieID)
        {
            Guid? userID = _currentUser.Id;
            var watchlist = (await _watchlistRepository.GetListAsync()).FirstOrDefault();

            if (watchlist == null)
            {
                watchlist = new Watchlist();
                await _watchlistRepository.InsertAsync(watchlist);
            }

            var serie = await _serieRepository.GetAsync(serieID);

            if (!watchlist.SerieList.Any(s => s.idSerie == serie.idSerie))
            {
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
