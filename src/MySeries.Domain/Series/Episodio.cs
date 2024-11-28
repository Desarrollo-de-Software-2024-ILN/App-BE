using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
namespace MySeries.Series
{
    public class Episodio : Entity<int>
    {
        public int NumEpisodio { get; set; }
        public DateOnly FechaEstreno { get; set; }
        public string Titulo { get; set; }


        //Foreign key
        public int TemporadaID { get; set; }
        public Temporada Temporada { get; set; }
    }
}
