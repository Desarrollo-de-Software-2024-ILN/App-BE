using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MySeries.Notifications
{
    public interface INotificationRepository : IRepository<Notification, int>
    {
        Task<List<Notification>> GetNotificacionesNoLeidasAsync(int usuarioID);

    }
}
