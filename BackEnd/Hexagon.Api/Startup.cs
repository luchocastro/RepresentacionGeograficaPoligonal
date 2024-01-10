using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Hexagon.Services;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Hexagon.Services.Interfaces;
using Hexagon.Api.Config;
using Hexagon.API.Boostrap;
using AutoMapper;
using Hexagon.Model.Mappings;
using Hexagon.Model.Models;
using Hexagon.Shared.DTOs;
using Hexagon.Model;
using Hexagon.Model.FileDataManager;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Hexagon.Api
{

    public class Startup
    {
        private const string enUSCulture = "en-US";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
 
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsOptions(Configuration);
            services.AddControllers();
 
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingModelProfile()); ;
            }); ;
            
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddDependencies(Configuration);
            services.AddSwaggerOptions();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
 
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCorsConfiguration();

            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwaggerGen(env);

        }
    }
}
