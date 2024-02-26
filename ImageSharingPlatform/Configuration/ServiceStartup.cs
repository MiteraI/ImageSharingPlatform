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

            return services;
        }
    }
}
