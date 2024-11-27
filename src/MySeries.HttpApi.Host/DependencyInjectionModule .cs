using Autofac;
using MySeries.Application; // Ajusta el namespace según la ubicación de IUserRepository
using MySeries.Infrastructure.Repositories; // Ajusta el namespace según la implementación concreta

namespace MySeries;
public class DependencyInjectionModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserRepository>()
               .As<IUserRepository>()
               .InstancePerLifetimeScope(); // Similar a AddScoped en el contenedor de DI estándar
    }
}
