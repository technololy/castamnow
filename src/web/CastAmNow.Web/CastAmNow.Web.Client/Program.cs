using Blazored.LocalStorage;
using Blazored.Modal;
using CastAmNow.Sdk;
using CastAmNow.UI.Services;
using CastAmNow.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register the FormFactor service
builder.Services.AddTransient<IFormFactor, FormFactor>();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<IDefectApi, DefectApi>();

await builder.Build().RunAsync();
