using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using MySeries.Series;
using MySeries.Watchlists;
using System.Reflection.Emit;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Users;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System;
using Volo.Abp.Auditing;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySeries.User;
using MySeries.Domain.Notificaciones;

namespace MySeries.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class MySeriesDbContext :
    AbpDbContext<MySeriesDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    //Serie
    public DbSet<Serie> Series { get; set; }

    //Temporada
    public DbSet<Temporada> Temporadas { get; set; }

    //Episodio
    public DbSet<Episodio> Episodios { get; set; }

    //Lista de seguimiento
    public DbSet<Watchlist> Watchlists { get; set; }

    //Notificación
    public DbSet<Notificacion> Notificaciones { get; set; }

    private readonly CurrentUserService _currentUserService;

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public MySeriesDbContext(DbContextOptions<MySeriesDbContext> options)
        : base(options)
    {
        _currentUserService = this.GetService<CurrentUserService>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //// Configuración del filtro global para CreatorId basado en el usuario actual
        builder.Entity<Serie>().HasQueryFilter(serie => serie.CreatorId == _currentUserService.GetCurrentUserId());

        /* Include modules to your migration db context */
        builder.Entity<Serie>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "series",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Title).IsRequired().HasMaxLength(128);
            b.Property(x => x.Genre).IsRequired().HasMaxLength(128);
            b.Property(x => x.Descripcion).IsRequired().HasMaxLength(512);
        });

        builder.Entity<Watchlist>(b => 
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Watchlist",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
        });

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        /* Configure your own tables/entities inside here */

        builder.Entity<Serie>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Series",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Title).IsRequired().HasMaxLength(128);
            b.Property(x => x.Gender).IsRequired().HasMaxLength(128);
            b.Property(x => x.Tipo).IsRequired().HasMaxLength(128);
            b.Property(x => x.TotalTemporadas).IsRequired(); // No aplica HasMaxLength porque es un int
            // Relación con Temporadas
            b.HasMany(s => s.Temporadas)
             .WithOne(t => t.Serie)
             .HasForeignKey(t => t.SerieID)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
        });
        //Temporada
        builder.Entity<Temporada>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Temporadas",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Titulo).IsRequired().HasMaxLength(128);
            b.Property(x => x.FechaLanzamiento).IsRequired().HasMaxLength(128);
            b.Property(x => x.NumTemporada).IsRequired();

            // Relación con Serie
            b.HasOne(t => t.Serie)
             .WithMany(s => s.Temporadas)
             .HasForeignKey(t => t.SerieID)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            // Relación con Episodios
            b.HasMany(t => t.Episodios)
             .WithOne(e => e.Temporada)
             .HasForeignKey(e => e.TemporadaID)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
        });
        //Episodio
        builder.Entity<Episodio>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Episodios",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Titulo).IsRequired().HasMaxLength(128);
            b.Property(x => x.FechaEstreno).IsRequired();
            b.Property(x => x.NumEpisodio).IsRequired();
            b.HasOne(e => e.Temporada)
             .WithMany(t => t.Episodios)
             .HasForeignKey(e => e.TemporadaID)
             .OnDelete(DeleteBehavior.Cascade);
        });

        //Lista de seguimiento
        builder.Entity<Watchlist>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "ListasDeSeguimiento",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.FechaModificacion).IsRequired();

        });

        //Notificación
        builder.Entity<Notificacion>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Notificacion",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.UsuarioId).IsRequired();
            b.Property(x => x.Titulo).IsRequired();
            b.Property(x => x.Msj).IsRequired();
            b.Property(x => x.Leida).IsRequired();
            b.Property(x => x.Tipo).IsRequired();
            b.Property(x => x.FechaCreacion).IsRequired();

        });


        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(MySeriesConsts.DbTablePrefix + "YourEntities", MySeriesConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
