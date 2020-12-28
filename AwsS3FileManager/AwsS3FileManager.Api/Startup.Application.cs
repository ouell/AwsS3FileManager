using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AwsS3FileManager.Api
{
    public partial class Startup
    {
        private void ConfigureApplication(IServiceCollection serivces)
        {
            serivces.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            serivces.AddAWSService<IAmazonS3>();
        }
    }
}