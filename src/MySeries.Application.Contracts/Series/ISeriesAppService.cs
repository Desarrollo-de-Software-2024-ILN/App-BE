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
        Task<ICollection<SerieDto>> SearchAsync(string? title, string? gender);

        Task CalificarSerieAsync(CalificacionDto input);
    }

}
