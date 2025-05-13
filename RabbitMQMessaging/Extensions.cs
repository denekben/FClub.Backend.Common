using FClub.Backend.Common.RabbitMQMessaging.Publisher;
using FClub.Backend.Common.RabbitMQMessaging.Subscriber;
using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.RabbitMQMessaging
{
    public static class MessagesExtensions
    {
        public static IServiceCollection AddCustomRabbitMq(
            this IServiceCollection services,
            RabbitMqConnectionSettings connectionSettings)
        {
            services.AddSingleton(connectionSettings);

            return services;
        }

        public static IServiceCollection AddCustomRabbitMqPublisher(
            this IServiceCollection services,
            PublisherOptions publisherOptions)
        {
            services.AddSingleton(publisherOptions);
            services.AddSingleton<IMessageBusPublisher, RabbitMqPublisher>();

            return services;
        }

        public static IServiceCollection AddCustomRabbitMqSubscriber(
            this IServiceCollection services,
            SubscriberOptions subscriberOptions)
        {
            services.AddSingleton(subscriberOptions);
            services.AddHostedService<MessageBusSubscriber>();
            services
                .Scan(s => s.FromAssemblies(subscriberOptions.Assembly)
                .AddClasses(c => c.AssignableTo(typeof(IMessageHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddCustomRabbitMq(
            this IServiceCollection services,
            Action<RabbitMqConnectionSettings> configureConnection)
        {
            var connectionSettings = new RabbitMqConnectionSettings();
            configureConnection(connectionSettings);

            services.AddCustomRabbitMq(connectionSettings);

            return services;
        }

        public static IServiceCollection AddCustomRabbitMqPublisher(
            this IServiceCollection services,
            Action<PublisherOptions> configurePublisherOptions)
        {
            var publisherOptions = new PublisherOptions();
            configurePublisherOptions.Invoke(publisherOptions);

            services.AddCustomRabbitMqPublisher(publisherOptions);

            return services;
        }

        public static IServiceCollection AddCustomRabbitMqSubscriber(
            this IServiceCollection services,
            Action<SubscriberOptions> configureSubscriberOptions)
        {
            var subscriberOptions = new SubscriberOptions();
            configureSubscriberOptions.Invoke(subscriberOptions);

            services.AddCustomRabbitMqSubscriber(subscriberOptions);

            return services;
        }
    }
}
