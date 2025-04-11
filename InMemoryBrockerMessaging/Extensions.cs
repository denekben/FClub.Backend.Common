using FClub.Backend.Common.InMemoryBrokerMessaging.Events;
using FClub.Backend.Common.InMemoryBrokerMessaging.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.InMemoryBrockerMessaging
{
    public static class Extensions
    {
        public static IServiceCollection AddInMemoryMessageBroker(this IServiceCollection services)
        {
            services.AddEvents();
            services.AddMessaging();

            return services;
        }
    }
}
