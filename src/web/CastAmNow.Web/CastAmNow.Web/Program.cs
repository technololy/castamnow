using CastAmNow.Sdk;
using CastAmNow.UI.Services;
using CastAmNow.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddTransient<IFormFactor, FormFactor>();
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
