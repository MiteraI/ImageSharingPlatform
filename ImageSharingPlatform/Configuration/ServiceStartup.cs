using ImageSharingPlatform.Service.Services;
using ImageSharingPlatform.Service.Services.Interfaces;

namespace ImageSharingPlatform.Configuration
{
    public static class ServiceStartup
    {
        public static IServiceCollection AddServiceModule(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IImageCategoryService, ImageCategoryService>();
            services.AddScoped<ISharedImageService, SharedImageService>();
            services.AddScoped<IImageRequestService, ImageRequestService>();
            services.AddScoped<ISubscriptionPackageService, SubscriptionPackageService>();
            services.AddScoped<IOwnedSubscriptionService, OwnedSubscriptionService>();
            return services;
        }
    }
}
