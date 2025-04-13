using FClub.Backend.Common.Services.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.Services
{
    public static class Exceptions
    {
        public static IServiceCollection AddCustomTokenService(
            this IServiceCollection services,
            TokenServiceOptions options)
        {
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton(options);

            return services;
        }

        public static IServiceCollection AddCustomTokenService(
            this IServiceCollection services,
            Action<TokenServiceOptions> configureOptions)
        {
            var options = new TokenServiceOptions();
            configureOptions(options);
            return services.AddCustomTokenService(options);
        }
    }
}
