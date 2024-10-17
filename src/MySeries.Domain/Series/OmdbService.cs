using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public class OmdbService : ISeriesApiService
    {
        public async Task<SerieDto[]> GetSeriesAsync(string title, string gender)
        {
            SerieDto[] series = new SerieDto[]
            {
                new SerieDto { Title = "Breaking Bad"}

            };
            return await Task.FromResult(series);
        }
    }   
}
