using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Configuration
{
    public static class VnpayStartup
    {
        public static IServiceCollection AddVnpayModule(this IServiceCollection services)
        {
            services.AddScoped<IVnpayService, VnpayService>();
            return services;
        }
    }
}
