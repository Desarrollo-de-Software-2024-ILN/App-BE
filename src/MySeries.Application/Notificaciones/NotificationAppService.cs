using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySeries.Application.Contracts.Notificaciones;
using MySeries.Domain.Notificaciones;
using Volo.Abp.DependencyInjection;

namespace MySeries.Notificaciones
{
    public class NotificationAppService: INotificationService, ITransientDependency
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEnumerable<INotificator> _notificators;

        public NotificationAppService(INotificationRepository notificationRepository, IEnumerable<INotificator> notificators)
        {
            _notificationRepository = notificationRepository;
            _notificators = notificators;
        }


        public async Task CrearNotificacionesAsync(int UsuarioId, string titulo, string msj, TipoNotificacion tipo)
        {
            // crear Dto
            var notificacionesDto = new NotificacionDto
            {
                UsuarioId = UsuarioId,
                Titulo = titulo,
                Msj = msj,
                Leida = false,
                Tipo = tipo
            };

            //agregar notif al repo
            var notificacion = new Notificacion
            {
                UsuarioId = UsuarioId,
                Titulo = titulo,
                Msj = msj,
                Leida = false,
                Tipo = tipo
            };

            await _notificationRepository.InsertAsync(notificacion);

            //envio notificadores
            var notificadoresFiltros = _notificators.Where(n => n.PuedeEnviar(tipo));
            foreach (var notificator in notificadoresFiltros)
            {
                await notificator.EnvioNotificacionesAsync(notificacionesDto);
            }

        }
    }
}
