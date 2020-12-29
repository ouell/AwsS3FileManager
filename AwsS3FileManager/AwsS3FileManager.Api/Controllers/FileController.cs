using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using AwsS3FileManager.Api.Model;
using AwsS3FileManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwsS3FileManager.Api.Controllers
{
    /// <summary>
    /// File controller
    /// </summary>
    [ApiController]
    [Route("api/file")]
    public class FileController : ControllerBase

    {
        /// <summary>
        /// Aws bucket service
        /// </summary>
        private readonly AwsBucketService _awsBucketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="awsBucketService"></param>
        public FileController(AwsBucketService awsBucketService)
        {
            _awsBucketService = awsBucketService;
        }

        /// <summary>
        /// Upload a file to s3 bucket
        /// </summary>
        /// <param name="request">Request data for upload file</param>
        [HttpPost("upload")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Upload([FromForm] AwsFileUploadRequestDto request)
        {
            try
            {
                await _awsBucketService.Upload(request.File, request.Tag);
                return Ok();
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode((int) e.StatusCode, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// List all files
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AwsFileModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListFiles()
        {
            try
            {
                var response = await _awsBucketService.ListFiles();
                return Ok(response);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode((int) e.StatusCode, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="fileName">File name</param>
        [HttpGet("download/{fileName}")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Download([FromRoute] string fileName)
        {
            try
            {
                var response = await _awsBucketService.Download(fileName);
                return File(response, "application/octet-stream", fileName);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode((int) e.StatusCode, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}