using Azure.Storage.Blobs;
using CastAmNow.Sdk.Abstractions;
using CastAmNow.Sdk.Implementations;
using Refit;

namespace CastAmNow.Sdk
{
    public class DefectApi(HttpClient defectClient, BlobContainerClient? blobServiceClient = default) : IDefectApi
    {
        public IDefectService DefectService => RestService.For<IDefectService>(defectClient);

        public IStorageUploadService StorageUploadService => new MicrosoftStorageUploadService(blobServiceClient);
    }
}
