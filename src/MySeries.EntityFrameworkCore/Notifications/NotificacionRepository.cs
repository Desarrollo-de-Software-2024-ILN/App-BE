using Microsoft.EntityFrameworkCore;
using MySeries.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MySeries.Notifications
{
    public class NotificacionRepository : EfCoreRepository<MySeriesDbContext, Notification, int>, INotificationRepository
    {
        public NotificacionRepository(IDbContextProvider<MySeriesDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Notification>> GetNotificacionesNoLeidasAsync(int UsuarioID)
        {
            return await DbSet
                .Where(n => n.UsuarioID == UsuarioID && !n.Leida)
                .ToListAsync();
        }

    }
}
