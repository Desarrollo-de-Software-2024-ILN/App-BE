using MySeries.Application.Contracts.Notificaciones;
using MySeries.Notificaciones;
using System.Threading.Tasks;
public class NotificadorPantalla : INotificator
{
    public bool PuedeEnviar(TipoNotificacion tipo) => tipo == TipoNotificacion.Pantalla;

    public Task EnvioNotificacionesAsync(NotificacionDto notificacionDTO)
    {
        return Task.CompletedTask;
    }
}
   

