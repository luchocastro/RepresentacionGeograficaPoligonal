using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hexagon.Services;
using Hexagon.Services.Interfaces;

using Hexagon.Core.Configuration;
using Hexagon.Model.Models;
using Microsoft.EntityFrameworkCore;
using Hexagon.Model.Repository;
using Hexagon.Model.FileDataManager;
using Hexagon.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Hexagon.Services.Helpers;
using Microsoft.AspNetCore.Http;

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
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddAuthorization();
            services.AddScoped<IDataRepository<UserDTO, User>, UserFileDataManager<UserDTO, User>>();
            services.AddHttpContextAccessor();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<Hexagon.Services.Interfaces.IHexAuthenticationService, HexAuthenticationService>();
            services.AddScoped<IFormulasResumen, FormulasResumenService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped < IAuthenticated, HexAuthenticationService>();
            //services.AddScoped< IAuthenticationService , HexAuthentication > ();
        }
    }
}
