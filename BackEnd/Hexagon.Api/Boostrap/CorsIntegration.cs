using Hexagon.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Hexagon.API.Boostrap
{
    public static class CorsIntegrationExtension
    {
        private const string CORS_POLICY = "AllowOrigin";

        public static void AddCorsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<Settings>();
            List<string> corsAllowedSites = GetAllowedHost(settings);

            services.AddCors(options =>
            {
                options.AddPolicy(
                   name: CORS_POLICY,
                   builder =>
                   {
                       builder//.WithOrigins(corsAllowedSites.ToArray())
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                   });
            });

            Console.WriteLine("CORS enabled for: " + string.Join(",", corsAllowedSites.ToArray()));
        }

        public static void UseCorsConfiguration(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseCors(CORS_POLICY);
        }

        private static List<string> GetAllowedHost(Settings settings)
        {
            var AllowedHosts = settings.CorsOptions.AllowedHosts?.Split(';');
            var AllowedPorts = settings.CorsOptions.AllowedPorts?.Split(';');
            var AllowedProtocols = settings.CorsOptions.AllowedProtocols?.Split(';');
            var corsAllowedSites = new List<string>();
            foreach (var host in AllowedHosts)
            {
                foreach (var protocol in AllowedProtocols)
                {
                    foreach (var port in AllowedPorts)
                    {
                        var finalPort = string.Empty;
                        if (!string.IsNullOrEmpty(port.Trim()))
                            finalPort = ":" + port.Trim();
                        corsAllowedSites.Add(protocol.Trim() + "://" + host.Trim() + finalPort);
                        corsAllowedSites.Add(protocol.Trim() + "://" + host.Trim() + finalPort + "/api");
                    }
                }
            }

            return corsAllowedSites;
        }
    }
}
