using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CastAmNow.UI.Services;
using CastAmNow.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register the FormFactor service
builder.Services.AddTransient<IFormFactor, FormFactor>();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
