using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Email.API
{
    static class Configurations
    {

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyName = Assembly.GetCallingAssembly().GetName();

            var version = string.Concat("v", assemblyName.Version);

            services.AddSwaggerGen(setup =>
            {
                var swaggerInfo = new OpenApiInfo
                {

                    Title = assemblyName.Name,
                    Version = version
                };

                setup.SwaggerDoc(swaggerInfo.Version, swaggerInfo);



                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                var XmlDocPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, string.Concat(PlatformServices.Default.Application.ApplicationName, ".xml"));

                if (File.Exists(XmlDocPath))
                    setup.IncludeXmlComments(XmlDocPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var version = string.Concat("v", Assembly.GetCallingAssembly().GetName().Version);

            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
            });

            return app;
        }
    }
}
