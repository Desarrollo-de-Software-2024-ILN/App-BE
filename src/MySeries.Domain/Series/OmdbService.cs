using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public class OmdbService : ISeriesService
    {
        public Task<SerieDto> GetSeriesAsync(string title, string gender)
        {
            return Task.FromResult(new SerieDto
            {
                Title = "Game oh Thrones",
            });
        }
    }   
}
