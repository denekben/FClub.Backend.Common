using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.HttpMessaging
{
    public static class Extensions
    {
        public static IServiceCollection AddHttpClientService(
            this IServiceCollection services,
            string name,
            HttpClientServiceOptions options)
        {
            services.AddHttpClient(name);
            services.AddKeyedScoped<IHttpClientService>(name, (provider, key) =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(name);
                return new HttpClientService(httpClient, options);
            });

            return services;
        }

        public static IServiceCollection AddHttpClientService(
            this IServiceCollection services,
            string name,
            Action<HttpClientServiceOptions> configureOptions)
        {
            var options = new HttpClientServiceOptions();
            configureOptions(options);
            return services.AddHttpClientService(name, options);
        }

        public static IServiceCollection AddHttpClientService(
            this IServiceCollection services,
            HttpClientServiceOptions options)
        {
            return services.AddHttpClientService("default", options);
        }

        public static IServiceCollection AddHttpClientService(
            this IServiceCollection services,
            Action<HttpClientServiceOptions> configureOptions)
        {
            return services.AddHttpClientService("default", configureOptions);
        }
    }
}
