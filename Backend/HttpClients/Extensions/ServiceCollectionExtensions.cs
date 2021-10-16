using System;
using HttpClients.Options;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClients.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBusClient(this IServiceCollection services,
            Action<EventBusClientOptions> configure)
        {
            services.Configure(configure);
            services.AddHttpClient<IEventBusClient, EventBusClient>();            
            
            return services;
        }
    }
}