using MySeries.Notificaciones;
using System.Threading.Tasks;

namespace MySeries.Application.Contracts.Notificaciones
{
    public interface INotificator
    {
        bool PuedeEnviar(TipoNotificacion tipo);

        public Task EnvioNotificacionesAsync(NotificacionDto notificacionDTO);
    }
}
