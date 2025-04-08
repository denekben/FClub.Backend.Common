using FClub.Backend.Common.BrokerMessaging.Publisher;
using FClub.Backend.Common.Events.Publisher;
using FClub.Backend.Common.Events.Subscriber;
using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.BrokerMessaging
{
    public static class MessagesExtensions
    {
        public static IServiceCollection AddRabbitMq(
            this IServiceCollection services,
            RabbitMqConnectionSettings connectionSettings,
            PublisherOptions publisherOptions,
            SubscriberOptions subscriberOptions)
        {
            services.AddSingleton(connectionSettings);
            services.AddSingleton(publisherOptions);
            services.AddSingleton(subscriberOptions);
            services.AddSingleton<IMessageBusPublisher, RabbitMqPublisher>();
            services.AddHostedService<MessageBusSubscriber>();

            return services;
        }

        public static IServiceCollection AddRabbitMq(
            this IServiceCollection services,
            Action<RabbitMqConnectionSettings> configureConnection,
            Action<PublisherOptions>? configurePublisherOptions = null,
            Action<SubscriberOptions>? configureSubscriberOptions = null)
        {
            var connectionSettings = new RabbitMqConnectionSettings();
            configureConnection(connectionSettings);

            var publisherOptions = new PublisherOptions();
            configurePublisherOptions?.Invoke(publisherOptions);

            var subscriberOptions = new SubscriberOptions();
            configureSubscriberOptions?.Invoke(subscriberOptions);

            services.AddRabbitMq(connectionSettings, publisherOptions, subscriberOptions);

            return services;
        }
    }
}
