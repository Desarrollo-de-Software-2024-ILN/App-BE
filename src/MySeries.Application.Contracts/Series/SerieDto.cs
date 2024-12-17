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
        public string Genre { get; set; }
        public string Descripcion { get; set; }
        public string IdSerie { get; set; }

        public ICollection<CalificationDto> Califications { get; set; }

    }
}
