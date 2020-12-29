using Amazon.S3;
using AwsS3FileManager.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AwsS3FileManager.Api
{
    public partial class Startup
    {
        private void ConfigureApplication(IServiceCollection serivces)
        {
            serivces.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            serivces.AddAWSService<IAmazonS3>();
            serivces.AddSingleton<AwsBucketService>();
            serivces.Configure<AwsSettings>(Configuration.GetSection("Aws"))
                    .AddSingleton(s => s.GetRequiredService<IOptions<AwsSettings>>().Value);
        }
    }
}