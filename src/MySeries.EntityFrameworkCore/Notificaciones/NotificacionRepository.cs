using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySeries.EntityFrameworkCore;
using MySeries.Notificaciones;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MySeries.Notificaciones
{
    public class NotificacionRepository(IDbContextProvider<MySeriesDbContext> dbContextProvider) : EfCoreRepository<MySeriesDbContext, Notificacion, int>(dbContextProvider), INotificationRepository
    {
        public async Task<List<Notificacion>> GetNotificacionesNoLeidasAsync(int usuarioId)
        {
            return await DbSet
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .ToListAsync();
        }
    }
}
