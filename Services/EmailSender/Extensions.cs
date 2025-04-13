using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FClub.Backend.Common.Services.EmailSender
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomEmailSender(
            this IServiceCollection services, EmailSenderOptions options)
        {
            services.AddSingleton(options);
            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }

        public static IServiceCollection AddCustomEmailSender(
            this IServiceCollection services, Action<EmailSenderOptions> configureOptions)
        {
            var options = new EmailSenderOptions();
            configureOptions(options);

            return services.AddCustomEmailSender(options);
        }
    }
}
