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

       // Usuario
        public Guid Creator { get; set; }

        public Guid CreatorId { get; set; }

        // Calificaciones
        public ICollection<Calificacion> calificaciones { get; set; }
        public Serie() 
        {
            calificaciones = new List<Calificacion>();
        }
    }
}
