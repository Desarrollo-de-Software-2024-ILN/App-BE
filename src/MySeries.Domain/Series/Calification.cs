using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Series
{
    public class Calification
    {
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaCreada { get; set; }

        public int IdSerie { get; set; }
        public Serie Serie { get; set; }

        public Guid User { get; set; }
    }
}
