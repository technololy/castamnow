using Azure.Storage.Blobs;
using CastAmNow.Sdk.Abstractions;
using CastAmNow.Sdk.Implementations;
using Refit;
using System.Text.Json;

namespace CastAmNow.Sdk
{
    public class DefectApi(HttpClient defectClient, BlobContainerClient? blobServiceClient = default) : IDefectApi
    {
        private readonly RefitSettings refitSettings = new()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })
        };
        public IDefectService DefectService => RestService.For<IDefectService>(defectClient,refitSettings);

        public IStorageUploadService StorageUploadService => new MicrosoftStorageUploadService(blobServiceClient);
    }
}
