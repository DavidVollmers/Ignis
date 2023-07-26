using Ignis.Components.Server;
using Ignis.Website;
using Ignis.Website.Server.Services;
using Ignis.Website.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IStaticFileService, StaticFileService>();
builder.Services.AddIgnisWebsite();
builder.Services.AddIgnisServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
