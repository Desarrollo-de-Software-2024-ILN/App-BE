using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Notifications
{
    public class NotificatorScreen : INotificator
    {
        public Boolean PuedeEnviar(TipoNotificacion tipo) => tipo == TipoNotificacion.Screen;

        public Task EnvioNotificationsAsync(NotificationDto notificationDto)
        {
            return Task.CompletedTask;
        }

    }
}
