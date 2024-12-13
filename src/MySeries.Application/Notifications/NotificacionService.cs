using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MySeries.Notifications
{
    public class NotificacionService : INotificationService, ITransientDependency
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEnumerable<INotificator> _notificators;

        public NotificacionService(INotificationRepository notificationRepository, IEnumerable<INotificator> notificators)
        {
            _notificationRepository = notificationRepository;
            _notificators = notificators;
        }

        public async Task GenerarYEnviarNotifAsync(int usuarioID, string titulo, string msj, TipoNotificacion tipo)
        {
            var notificationDto = new NotificationDto
            {
                UsuarioID = usuarioID,
                Titulo = titulo,
                Msj = msj,
                Leida = false,
                Tipo = tipo
            };

            var notification = new Notification
            {
                UsuarioID = usuarioID,
                Titulo = titulo,
                Msj = msj,
                Leida = false,
                Tipo = tipo
            };

            await _notificationRepository.InsertAsync(notification);

            var notificators = _notificators.Where(n => n.PuedeEnviar(tipo));
            foreach (var notificator in notificators)
            {
                await notificator.EnvioNotificationsAsync(notificationDto);
            }
        }
    }
}
