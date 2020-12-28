using System;

namespace AwsS3FileManager.Api.Model
{
    public class AwsFileModel
    {
        public AwsFileModel(string tag,
                            long fileSize,
                            string filePath,
                            string fileName,
                            DateTime lastModified)
        {
            Tag = tag;
            FileSize = fileSize;
            FilePath = filePath;
            FileName = fileName;
            LastModified = lastModified;
        }

        /// <summary>
        /// File tag
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// File size
        /// </summary>
        public long FileSize { get; set; }
        
        /// <summary>
        /// File path
        /// </summary>
        public string FilePath { get; set; }
        
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// Last modified
        /// </summary>
        public DateTime LastModified { get; set; }
    }

        
}