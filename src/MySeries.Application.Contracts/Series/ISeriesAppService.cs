using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;

namespace MySeries.Series
{
    public interface ISeriesAppService : ICrudAppService<SerieDto, int, PagedAndSortedResultRequestDto, CreateUpdateSerieDto>
    {
        Task<SerieDto[]> BuscarSerieAsync(string Title, string Genre = null);
        Task CalificarSerieAsync(CalificationDto input);
        Task ObtenerSeriesAsync(SerieDto[] serieDtos);
    }

}
