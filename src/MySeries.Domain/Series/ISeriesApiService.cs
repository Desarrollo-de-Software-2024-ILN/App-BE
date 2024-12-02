using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public interface ISeriesApiService
    {
        Task<SerieDto[]> BuscarSerieAsync(string title, string gender);

        Task<TemporadaDto> BuscarTemporadaAsync(int id, int numTemporada);
    }
}
