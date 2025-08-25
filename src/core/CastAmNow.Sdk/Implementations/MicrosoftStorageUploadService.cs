using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CastAmNow.Sdk.Abstractions;

namespace CastAmNow.Sdk.Implementations
{

    internal class MicrosoftStorageUploadService : IStorageUploadService
    {
        private readonly Progress<long> progress = new();
        private readonly string? connectionString;
        private readonly BlobServiceClient? blobServiceClient;
        private readonly BlobContainerClient? container;

        public MicrosoftStorageUploadService(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public MicrosoftStorageUploadService(BlobServiceClient? blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }
        public MicrosoftStorageUploadService(BlobContainerClient? blobContainerClient)
        {
            this.container = blobContainerClient;
        }
        public async Task<string> UploadImage(Stream stream, string userFolder, string fileName, EventHandler<double> OnProgressChanged, CancellationToken cancellationToken = default, string folder = "VillagesSquare")
        {
            try
            {
                await container!.CreateIfNotExistsAsync();
                var blob = container.GetBlobClient(fileName);
                container.SetAccessPolicy(PublicAccessType.Blob);
                var upload = blob.UploadAsync(stream, progressHandler: progress, cancellationToken: cancellationToken);
                progress.ProgressChanged += (s, e) => OnProgressChanged?.Invoke(this, Convert.ToDouble(e) / Convert.ToDouble(stream.Length) * 100.0);
                return blob.Uri.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
