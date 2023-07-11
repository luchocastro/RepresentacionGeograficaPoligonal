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
using Hexagon.Model;

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
            services.AddScoped<IDataRepository<ProyectDataDTO, ProyectData>, ProyectDataFileDataManager<ProyectDataDTO, ProyectData>>();
            services.AddScoped<IDataRepository<HexFileDTO, HexFile>, FileDataManager<HexFileDTO, HexFile>>();
            services.AddScoped<IDataRepository<AnalizedFileDTO, AnalizedFile>, FileDataManager<AnalizedFileDTO, AnalizedFile>>();
            services.AddScoped<IDataRepository<NativeFileDTO, NativeFile>, FileDataManager<NativeFileDTO, NativeFile>>();
            services.AddScoped<IDataRepository<LayoutDto , Layout>, FileDataManager<LayoutDto, Layout>>();
            services.AddScoped <IDataRepository<MapDefinitionDTO, MapDefinition>, FileDataManager<MapDefinitionDTO, MapDefinition>>();
            services.AddScoped<IDataRepository<HexagonDetailsDTO, HexagonDetails>, FileDataManager<HexagonDetailsDTO, HexagonDetails>>();
            services.AddScoped<IDataRepository<FunctionDTO, Function>, FileDataManager<FunctionDTO, Function>>();
            services.AddScoped<IDataRepository<DataFileConfigurationDTO, DataFileConfiguration>, DataFileConfigurationFileDataManager<DataFileConfigurationDTO, DataFileConfiguration>>();
            services.AddScoped<IDataRepository<CalculatedHexagonDTO, CalculatedHexagon>, FileDataManager<CalculatedHexagonDTO, CalculatedHexagon>>();
            services.AddScoped<IDataRepository<ColumnDTO, Column>, FileDataManager<ColumnDTO , Column >>();
            services.AddScoped<IDataRepository<PaletteClassDTO, PaletteClass>, FileDataManager<PaletteClassDTO, PaletteClass>>();
            services.AddScoped<IDataRepository<ContentDTO, Content>, FileDataManager<ContentDTO, Content>>();

            services.AddHttpContextAccessor();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<Hexagon.Services.Interfaces.IHexAuthenticationService, HexAuthenticationService>();
            services.AddScoped<IFormulasResumen, FormulasResumenService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped < IAuthenticated, HexAuthenticationService>();

            //services.AddScoped< IAuthenticationService , HexAuthentication > ();
            services.Configure<FileDataManagerOptions>(configuration.GetSection(IFileDataManagerOptions.Position));
            services.AddOptions();
            services.AddSingleton<IFileDataManagerOptions, PseudoServiceFileDataManagerOption>();
        }
    }
    
}
