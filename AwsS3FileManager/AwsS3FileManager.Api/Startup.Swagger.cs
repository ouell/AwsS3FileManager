using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AwsS3FileManager.Api
{
    /// <summary>
    ///     Class responsible for swagger config
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        ///     Configure swagger options
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var appXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, appXml);
                var apiInfo = Configuration.GetSection("SwaggerInfo").Get<OpenApiInfo>();

                options.SwaggerDoc("v1", apiInfo);
                options.IncludeXmlComments(filePath);
            });
        }

        /// <summary>
        ///     Configure swagger
        /// </summary>
        /// <param name="app"></param>
        private static void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AwsS3FileManager.Api v1"));
            app.UseSwagger();
        }
    }
}