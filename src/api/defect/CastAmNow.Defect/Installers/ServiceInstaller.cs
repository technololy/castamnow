using CastAmNow.Api.Infrastructure.Abstractions;
using CastAmNow.Defect.API.Abstractions;
using CastAmNow.Defect.API.Services;

namespace CastAmNow.Defect.API.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDefectService, DefectService>();
        }
    }
}
