using Ignis.Components.WebAssembly;
using Ignis.Website;
using Ignis.Website.Services;
using Ignis.Website.WebAssembly;
using Ignis.Website.WebAssembly.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IStaticFileService, StaticFileService>();
builder.Services.AddIgnisWebsite();
builder.Services.AddIgnisWebAssembly();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);
