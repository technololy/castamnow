using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CastAmNow.Api.Infrastructure.Abstractions
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
