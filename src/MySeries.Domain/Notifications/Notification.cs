using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace MySeries.Notifications
{
    public class Notification : FullAuditedEntity<int>
    {
        public int UsuarioID { get; set; }
        public string Titulo { get; set; }
        public string Msj { get; set; }
        public bool Leida { get; set; }
       public TipoNotificacion Tipo {  get; set; }
    }
}
