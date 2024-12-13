using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Notifications
{
    public interface INotificationService
    {
        Task GenerarYEnviarNotifAsync(int usuarioID, string titulo, string msj, TipoNotificacion tipo);
    }
}
