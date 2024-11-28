using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public class EpisodioDto 
    {
        public int NumEpisodio { get; set; }
        public DateOnly FechaEstreno { get; set; }
        public string Titulo { get; set; }

        //Foreign key
        public int TemporadaId { get; set; }
    }
}
