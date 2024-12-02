using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MySeries.Series
{
    public class Calificacion : Entity<int>
    {
        public float CalificacionNota { get; set; }
        public string? Comentario { get; set; }
        public DateTime FechaCalificacion { get; set; }

        //Foreign key
        public int SerieId { get; set; }
        public Serie Serie { get; set; }

        // Usuario
        public Guid UsuarioId { get; set; }
    }
}
