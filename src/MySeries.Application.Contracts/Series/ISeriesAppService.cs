using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace MySeries.Series
{
    public interface ISeriesAppService : ICrudAppService<SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto, CreateUpdateSerieDto>
    {
        Task<SerieDto[]> BuscarSerieAsync(string title, string? gender = null);

        Task<TemporadaDto> BuscarTemporadaAsync(int id, int numeTemporada);

        Task CalificarSerieAsync(CalificacionDto input);

        Task PersistirSeriesAsync(SerieDto[] seriesDto);
    }

}
