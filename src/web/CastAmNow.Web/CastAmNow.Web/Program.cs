using System.Net.Http.Headers;
using Azure.Storage.Blobs;
using Blazored.LocalStorage;
using Blazored.Modal;
using CastAmNow.Core.Services;
using CastAmNow.Sdk;
using CastAmNow.UI.Services;
using CastAmNow.Web.Services;
using CastAmNow.Sdk.Abstractions;
using CastAmNow.Sdk.Implementations;
using ILocalStorageService = CastAmNow.Core.Abstractions.ILocalStorageService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddTransient<IFormFactor, FormFactor>();
builder.Services.AddBlazoredModal();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorBootstrap();
builder.Services.AddTransient<ILocalStorageService, LocalStorageService>();
builder.Services.AddTransient<ICastedService, CastedService>();
builder.Services.AddHttpClient<IBackendApiService, BackendApiService>(
    client =>
    {
        client.BaseAddress = new Uri("https://localhost:5111/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    });
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.AddAzureBlobContainerClient("files");
builder.Services.AddHttpClient("DefectClient", cfg =>
{
    cfg.BaseAddress = new("https+http://api");
});

builder.Services.AddSingleton<IStorageUploadService>(sp =>
{
    var storageType = builder.Configuration["UPLOAD_STORAGE_TYPE"]?.ToLower() ?? "microsoft";
    if (storageType == "aws")
    {
        // AWS S3 Implementation
        var s3Config = new Amazon.S3.AmazonS3Config
        {
            ServiceURL = builder.Configuration["AWS_SERVICE_URL"] ?? "https://s3.shopeyin.com",
            AuthenticationRegion = builder.Configuration["AWS_REGION"] ?? "garage",
            ForcePathStyle = true,
        };
        var credentials = new Amazon.Runtime.BasicAWSCredentials(
            builder.Configuration["AWS_ACCESS_KEY"],
            builder.Configuration["AWS_SECRET_KEY"]
        );
        var s3Client = new Amazon.S3.AmazonS3Client(credentials, s3Config);
        return new AwsS3StorageUploadService(builder.Configuration["AWS_WEB_URL"] ?? "web.shopeyin.com", s3Client);
    }
    else
    {
        // Microsoft Azure Implementation (Default)
        var blobContainerClient = sp.GetRequiredService<BlobContainerClient>();
        return new MicrosoftStorageUploadService(blobContainerClient);
    }
});

builder.Services.AddSingleton<IDefectApi>(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var defectHttpClient = clientFactory.CreateClient("DefectClient");
    var storageService = sp.GetRequiredService<IStorageUploadService>();
    return new DefectApi(defectHttpClient, storageService);
});
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<CastAmNow.Web.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(CastAmNow.Web.Client._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(CastAmNow.UI._Imports).Assembly);

app.Run();
