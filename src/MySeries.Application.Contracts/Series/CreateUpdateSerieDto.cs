using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public class CreateUpdateSerieDto
    { 
        public string Title { get; set; }
        public string id { get; set; }
        public string Tipo { get; set; }
        public int TotalTemporadas { get; set; }
    }
}
