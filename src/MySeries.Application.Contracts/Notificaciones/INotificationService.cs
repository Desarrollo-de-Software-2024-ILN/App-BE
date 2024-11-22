using System;
using System.Threading.Tasks;

namespace MySeries.Notificaciones
{
    public interface INotificationService
    {
        Task CrearNotificacionesAsync(int UsuarioId, string titulo, string msj, TipoNotificacion tipo);
    }
}
