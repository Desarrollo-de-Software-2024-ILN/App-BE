using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace MySeries.Notificaciones
{
    public class Notificacion : FullAuditedEntity<int>
    {
        public int UsuarioId { get; set; }
        public required string Titulo { get; set; }
        public string Msj { get; set; }
        public bool Leida { get; set; }
        public TipoNotificacion Tipo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
