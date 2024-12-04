using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series
{
    public class Serie : AggregateRoot<int>
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Descripcion { get; set; }

    }
}
