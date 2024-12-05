using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Notificaciones
{
    public interface INotificationRepository :IRepository<Notificacion, int>
    {
        Task<List<Notificacion>> GetNotificacionesNoLeidasAsync(int usuarioId);
    }
}
