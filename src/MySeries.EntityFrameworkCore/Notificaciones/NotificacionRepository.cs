using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySeries.Domain.Notificaciones;
using MySeries.Notificaciones;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MySeries.EntityFrameworkCore.Notificaciones
{
    public class NotificacionRepository : EfCoreRepository<MySeriesDbContext, Notificacion, int>, INotificationRepository
    {
        public NotificacionRepository(IDbContextProvider<MySeriesDbContext> dbContextProvider) 
            : base(dbContextProvider) 
        { 
        }
        public async Task<List<Notificacion>> GetNotificacionesNoLeidasAsync(int usuarioId)
        {
            return await DbSet
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .ToListAsync();
        }
    }
}
