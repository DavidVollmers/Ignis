using Ignis.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Ignis.Tests.Common;

public static class IgnisTestExtensions
{
    public static IServiceCollection AddIgnisTestServices(this IServiceCollection serviceCollection)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddIgnis();
        serviceCollection.AddScoped<IHostContext, TestHostContext>();
        serviceCollection.AddSingleton<TimeProvider, TestTimeProvider>();

        return serviceCollection;
    }
}
