using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Blazored.Modal;
using CastAmNow.Core.Services;
using CastAmNow.Sdk;
using CastAmNow.UI.Services;
using CastAmNow.Web.Services;
using ILocalStorageService = CastAmNow.Core.Abstractions.ILocalStorageService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddTransient<IFormFactor, FormFactor>();
builder.Services.AddBlazoredModal();

builder.Services.AddBlazoredLocalStorage();
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
builder.Services.AddHttpClient("DefectClient", cfg =>
{
    cfg.BaseAddress = new("https+http://api");
});

builder.Services.AddSingleton<IDefectApi>(sdk =>
{
    var clientFactory = sdk.GetRequiredService<IHttpClientFactory>();
    var defectHttpClient = clientFactory.CreateClient("DefectClient");
    return new DefectApi(defectHttpClient);
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
