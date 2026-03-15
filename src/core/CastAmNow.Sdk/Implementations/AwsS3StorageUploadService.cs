using Amazon.S3;
using Amazon.S3.Model;
using CastAmNow.Sdk.Abstractions;

namespace CastAmNow.Sdk.Implementations
{
    public class AwsS3StorageUploadService(string webUrl, IAmazonS3 s3Client) : IStorageUploadService
    {
        public async Task<string> UploadImage(Stream stream, string folder, string fileName, EventHandler<double> OnProgressChanged, CancellationToken cancellationToken = default, string bucketName = "villagessquare")
        {
            try
            {
                var objectKey = $"{bucketName}/{folder}/{fileName}".Trim('/');

                var putRequest = new PutObjectRequest
                {
                    InputStream = stream,
                    Key = objectKey,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    DisablePayloadSigning = true,
                    UseChunkEncoding = false
                };

                putRequest.StreamTransferProgress += (s, e) =>
                {
                    OnProgressChanged?.Invoke(this, e.PercentDone);
                };

                await s3Client.PutObjectAsync(putRequest, cancellationToken);
                return $"https://{bucketName}.{webUrl}/{objectKey}";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
