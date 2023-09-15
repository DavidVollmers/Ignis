using Ignis.Components.Server;
using Ignis.Website;
using Ignis.Website.Server.Services;
using Ignis.Website.Services;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IStaticFileService, StaticFileService>();
builder.Services.AddIgnisWebsite();
builder.Services.AddIgnisServer();

// https://github.com/dotnet/aspnetcore/pull/45897
builder.Services.Configure<StaticFileOptions>(options =>
{
    var extensionProvider = new FileExtensionContentTypeProvider { Mappings = { [".razor"] = "text/plain" } };

    options.ContentTypeProvider = extensionProvider;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization(options =>
{
    options.AddSupportedCultures("en");
    options.AddSupportedUICultures("en");
    options.SetDefaultCulture("en");
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
