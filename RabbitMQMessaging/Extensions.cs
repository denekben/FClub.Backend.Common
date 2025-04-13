using FClub.Backend.Common.RabbitMQMessaging.Publisher;
using FClub.Backend.Common.RabbitMQMessaging.Subscriber;
using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.RabbitMQMessaging
{
    public static class MessagesExtensions
    {
        public static IServiceCollection AddCustomRabbitMq(
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

        public static IServiceCollection AddCustomRabbitMq(
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

            services.AddCustomRabbitMq(connectionSettings, publisherOptions, subscriberOptions);

            return services;
        }
    }
}
