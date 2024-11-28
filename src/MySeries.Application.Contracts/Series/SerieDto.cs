using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace MySeries.Series
{
    public class SerieDto : EntityDto<int>
    {
        public string Title { get; set; } 
        public string Generos { get; set; }
        public string id { get; set; }
        public string Tipo { get; set; }
        public int TotalTemporadas { get; set; }

        public ICollection<TemporadaDto> Temporadas { get; set; }

    }
}
