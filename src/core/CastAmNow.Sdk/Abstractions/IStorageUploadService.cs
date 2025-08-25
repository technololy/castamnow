namespace CastAmNow.Sdk.Abstractions
{
    public interface IStorageUploadService
    {
        Task<string> UploadImage(Stream stream, string userFolder, string fileName, EventHandler<double> OnProgressChanged, CancellationToken cancellationToken = default, string folder = "VillagesSquare");
    }
}
