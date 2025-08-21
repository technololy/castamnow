using CastAmNow.Api.Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CastAmNow.Api.Infrastructure.Extensions
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly<T>(this IServiceCollection service, IConfiguration configuration)
        {
            var installers = typeof(T).Assembly.ExportedTypes.Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installers.ForEach(installer => installer.InstallServices(service, configuration));
        }
    }
}
