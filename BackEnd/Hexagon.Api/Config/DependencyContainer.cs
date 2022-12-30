using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hexagon.Services;
using Hexagon.Services.Interfaces;

using Hexagon.Core.Configuration;
using Hexagon.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Hexagon.Api.Config
{
    public static class DependencyContainer
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup));


            // Add Core Layer
            services.Configure<Settings>(configuration);
            services.AddDbContext<DataContext>(options => options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFormulasResumen, FormulasResumenService>(); 
        }
    }
}
