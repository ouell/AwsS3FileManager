using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AwsS3FileManager.Api
{
    public partial class Startup
    {
        private static void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            }).AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }

        private static void ConfigureControllers(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}