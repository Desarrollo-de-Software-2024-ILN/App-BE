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
using MySeries.Notifications;
using MySeries.Users;

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
    public DbSet<Serie> Series { get; set; } 

    public DbSet<Watchlist> Watchlists { get; set; }

    public DbSet<Notification> Notification { get; set; }

    public DbSet<Calification> Calification { get; set; }

    private readonly ICurrentUserService _currentUserService;

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

    public MySeriesDbContext(DbContextOptions<MySeriesDbContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

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

        //filtro global por usuario 
        builder.Entity<Serie>().HasQueryFilter(serie => serie.CreatorId == _currentUserService.GetCurrentUserID());

        // serie
        builder.Entity<Serie>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Series",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Title).IsRequired().HasMaxLength(128);
            b.Property(x => x.Genre).IsRequired().HasMaxLength(64);
            b.Property(x => x.IdSerie).IsRequired().HasMaxLength(16);
            b.Property(x => x.Descripcion).IsRequired().HasMaxLength(256);
            b.HasMany(s => s.Califications)
            .WithOne(c => c.Serie)
            .HasForeignKey(c => c.IdSerie)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        });

        //watchlist
        builder.Entity<Watchlist>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Watchlists",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.FechaModificacion).IsRequired();
        });

        //notification
        builder.Entity<Notification>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Notifications",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.UsuarioID).IsRequired();
            b.Property(x => x.Titulo).IsRequired();
            b.Property(x => x.Msj).IsRequired();
            b.Property(x => x.Leida).IsRequired();
            b.Property(x => x.Tipo).IsRequired();
        });

        //calification
        builder.Entity<Calification>(b =>
        {
            b.ToTable(MySeriesConsts.DbTablePrefix + "Calificacion",
                MySeriesConsts.DbSchema);
            b.ConfigureByConvention(); 
            b.Property(x => x.Nota).IsRequired();
            b.Property(x => x.Comentario);
            b.Property(x => x.FechaCreada).IsRequired();
            b.Property(x => x.IdSerie).IsRequired();
            b.Property(x => x.User).IsRequired();
            b.HasOne<Serie>(c => c.Serie)
            .WithMany(s => s.Califications)
            .HasForeignKey(c => c.IdSerie)
            .IsRequired(false); 

        });

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(MySeriesConsts.DbTablePrefix + "YourEntities", MySeriesConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
