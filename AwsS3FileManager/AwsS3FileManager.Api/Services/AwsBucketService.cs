using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AwsS3FileManager.Api.Model;
using Microsoft.AspNetCore.Http;

namespace AwsS3FileManager.Api.Services
{
    /// <summary>
    /// Class responsible for aws services
    /// </summary>
    public class AwsBucketService
    {
        /// <summary>
        /// Aws settings
        /// </summary>
        private readonly AwsSettings _awsSettings;

        /// <summary>
        /// Amazon s3 client
        /// </summary>
        private readonly IAmazonS3 _s3Client;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s3Client"></param>
        /// <param name="awsSettings"></param>
        public AwsBucketService(IAmazonS3 s3Client,
                                AwsSettings awsSettings)
        {
            _s3Client = s3Client;
            _awsSettings = awsSettings;
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="file">File <seealso cref="IFormFile" /></param>
        /// <param name="tag">File tag</param>
        public async Task Upload(IFormFile file,
                                 string tag)
        {
            var fileTransfer = new TransferUtility(_s3Client);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                Key = file.FileName,
                ContentType = file.ContentType,
                InputStream = file.OpenReadStream(),
                BucketName = _awsSettings.BucketName
            };
            uploadRequest.Metadata.Add("x-amz-meta-tag", tag);

            await fileTransfer.UploadAsync(uploadRequest);
        }

        /// <summary>
        /// List files information
        /// </summary>
        /// <returns>List of file information <seealso cref="IEnumerable{AwsFileModel}"/></returns>
        public async Task<IEnumerable<AwsFileModel>> ListFiles()
        {
            var request = new ListObjectsV2Request
            {
                MaxKeys = 50,
                Prefix = "/",
                Delimiter = "*",
                BucketName = _awsSettings.BucketName
            };

            ListObjectsV2Response response;
            var listFile = new List<AwsFileModel>();
            do
            {
                response = await _s3Client.ListObjectsV2Async(request);
                foreach (var s3Object in response.S3Objects)
                {
                    var metadataRequest = new GetObjectMetadataRequest
                    {
                        Key = s3Object.Key,
                        BucketName = _awsSettings.BucketName
                    };

                    var metadataRespose = await _s3Client.GetObjectMetadataAsync(metadataRequest);
                    var tag = metadataRespose.Metadata["tag"];

                    listFile.Add(new AwsFileModel(tag,
                                                  s3Object.Size,
                                                  s3Object.Key.Split("/").First(),
                                                  s3Object.Key.Split("/").Last(),
                                                  s3Object.LastModified));
                }

                request.ContinuationToken = response.NextContinuationToken;
            } while (response.IsTruncated);

            return listFile;
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="fileName">File name</param>
        public async Task Delete(string fileName)
        {
            var request = new DeleteObjectRequest
            {
                Key = fileName,
                BucketName = _awsSettings.BucketName
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>File in byte array</returns>
        public async Task<byte[]> Download(string fileName)
        {
            var transferUtility = new TransferUtility(_s3Client);
            var request = new GetObjectRequest
            {
                Key = fileName,
                BucketName = _awsSettings.BucketName
            };

            var s3ObjectResponse = await transferUtility.S3Client.GetObjectAsync(request);
            await using var ms = new MemoryStream();
            await s3ObjectResponse.ResponseStream.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}