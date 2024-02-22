using ImageSharingPlatform.Service.Services.Interfaces;
using ImageSharingPlatform.Service.Services;

namespace ImageSharingPlatform.Configuration
{
    public static class AzureBlobStartup
    {
        public static IServiceCollection AddAzureBlobModule(this IServiceCollection services)
        {
            services.AddSingleton<IAzureBlobService, AzureBlobService>();
            return services;
        }
    }
}
