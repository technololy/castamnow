using CastAmNow.Api.Infrastructure.Abstractions;
using CastAmNow.Defect.API.Repositories;

namespace CastAmNow.Defect.API.Installers
{
    public class DBInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository<Domain.Defect.Defect>, DefectRepository>();
        }
    }
}
