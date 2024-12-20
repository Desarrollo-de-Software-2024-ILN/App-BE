using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySeries.Notifications
{
    public class NotificatorMail : INotificator
    {
        public bool PuedeEnviar(TipoNotificacion tipo)
        {
            return tipo == TipoNotificacion.Mail;
        }

        public async Task EnvioNotificationsAsync(NotificationDto notificationDto)
        {
            var notification = new Notification
            {
                UsuarioID = notificationDto.UsuarioID,
                Titulo = notificationDto.Titulo,
                Msj = notificationDto.Msj,
                Leida = false,
                Tipo = notificationDto.Tipo,
            };

            Console.WriteLine($"Email enviado a {notification.UsuarioID}: {notification.Titulo} - {notification.Msj}");
        }
    }
}