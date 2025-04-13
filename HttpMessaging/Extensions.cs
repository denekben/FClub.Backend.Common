using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.HttpMessaging
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomHttpClientService(
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

        public static IServiceCollection AddCustomHttpClientService(
            this IServiceCollection services,
            string name,
            Action<HttpClientServiceOptions> configureOptions)
        {
            var options = new HttpClientServiceOptions();
            configureOptions(options);
            return services.AddCustomHttpClientService(name, options);
        }

        public static IServiceCollection AddCustomHttpClientService(
            this IServiceCollection services,
            HttpClientServiceOptions options)
        {
            return services.AddCustomHttpClientService("default", options);
        }

        public static IServiceCollection AddCustomHttpClientService(
            this IServiceCollection services,
            Action<HttpClientServiceOptions> configureOptions)
        {
            return services.AddCustomHttpClientService("default", configureOptions);
        }
    }
}
