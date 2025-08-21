using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CastAmNow.UI.Services;
using CastAmNow.Web.Client.Services;
using CastAmNow.Sdk;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register the FormFactor service
builder.Services.AddTransient<IFormFactor, FormFactor>();
builder.Services.AddTransient<IDefectApi, DefectApi>();

await builder.Build().RunAsync();
