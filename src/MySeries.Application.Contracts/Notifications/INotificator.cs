using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Notifications
{
    public interface INotificator
    {
        bool PuedeEnviar(TipoNotificacion tipo);
        Task EnvioNotificationsAsync(NotificationDto notificationDto);
    }
}
