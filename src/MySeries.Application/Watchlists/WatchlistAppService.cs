
using MySeries.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Watchlists
{
    public class WatchlistAppService : ApplicationService, IWatchlistAppService
    {
        private readonly IRepository<Watchlist, int> _watchlistRepository;
        private readonly IRepository<Serie, int> _serieRepository;

        public WatchlistAppService(IRepository<Watchlist, int> watchlistRepository, IRepository<Serie, int> serieRepository)
        {
            _serieRepository = serieRepository;
            _watchlistRepository = watchlistRepository;
        }

        public async Task AddSerieAsync(int serieId)
        {
            var watchlist = ((List<Watchlist>)await _watchlistRepository.GetListAsync()).FirstOrDefault();
            

            if (watchlist == null)
            {
                watchlist = new Watchlist();
                await _watchlistRepository.InsertAsync(watchlist);

            };

            var serie = await _serieRepository.GetAsync(serieId);
            watchlist.Series.Add(serie);

            await _watchlistRepository.UpdateAsync(watchlist);

        }
    }
}
