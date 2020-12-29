using Microsoft.AspNetCore.Http;

namespace AwsS3FileManager.Api.Model
{
    public class AwsFileUploadRequestDto
    {
        public IFormFile File { get; set; }
        public string Tag { get; set; }
    }
}