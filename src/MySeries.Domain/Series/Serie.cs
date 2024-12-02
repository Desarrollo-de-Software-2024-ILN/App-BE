using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace MySeries.Series
{
    public class Serie : AggregateRoot<int>, IMustHaveCreator<Guid>
    {
        public string Title { get; set; }

        public string Gender { get; set; }
        public string Id { get; set; }
        public string Tipo { get; set; }
        public int TotalTemporadas { get; set; }
        public ICollection<Temporada> Temporadas { get; set; }

       // Usuario
        public Guid Creator { get; set; }

        public Guid CreatorId { get; set; }

        // Calificaciones
        public ICollection<Calificacion> Calificaciones { get; set; }
        public Serie()
        {
            Calificaciones = new List<Calificacion>();
        }
    }
}
