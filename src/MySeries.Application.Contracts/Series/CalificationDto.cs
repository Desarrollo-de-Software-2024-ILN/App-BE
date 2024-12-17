using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MySeries.Series
{
    public class CalificationDto
    {
        [Range(1,10, ErrorMessage = "Nota entre 1 y 10")]

        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaCreada { get; set; }

        public int IdSerie { get; set; }

        public Guid User { get; set; }
    }
}
