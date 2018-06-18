using Microsoft.Extensions.DependencyInjection;

namespace Inquisition.Reporting.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddReporter(this IServiceCollection services)
        {
            services.AddSingleton<ReporterBuilder>();
            services.AddSingleton<Reporter>();

            return services;
        }
    }
}
