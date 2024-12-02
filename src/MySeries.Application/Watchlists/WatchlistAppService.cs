using MySeries.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        public async Task AddSerieAsync(int SerieID)
        {
            var listaDeSeguimiento = ((List<Watchlist>)await _watchlistRepository.GetListAsync()).FirstOrDefault();

            if (listaDeSeguimiento == null)
            {
                listaDeSeguimiento = new Watchlist();
                await _watchlistRepository.InsertAsync(listaDeSeguimiento);
            }

            var Serie = await _serieRepository.GetAsync(SerieID);
            listaDeSeguimiento.Series.Add(Serie);
            await _watchlistRepository.UpdateAsync(listaDeSeguimiento);

        }
    }
}
