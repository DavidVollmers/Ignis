﻿using Ignis.Components;
using Ignis.Components.Web;
using Ignis.Website.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Website;

public static class IgnisWebsiteExtensions
{
    public static IServiceCollection AddIgnisWebsite(this IServiceCollection services)
    {
        services.AddLocalization();
        
        services.AddIgnisWeb(); 
        services.AddIgnis();

        services.AddScoped<IPageService, PageService>();

        return services;
    }
}