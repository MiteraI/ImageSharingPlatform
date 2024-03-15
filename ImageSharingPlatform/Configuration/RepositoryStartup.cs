using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Repository.Repositories;

namespace ImageSharingPlatform.Configuration
{
    public static class RepositoryStartup
    {
        public static IServiceCollection AddReposiotoryModule(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISharedImageRepository, SharedImageRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IImageCategoryRepository, ImageCategoryRepository>();
            services.AddScoped<IImageRequestRepository, ImageRequestRepository>();
            services.AddScoped<ISubscriptionPackageRepository, SubscriptionPackageRepository>();
            services.AddScoped<IRequestDetailRepository, RequestDetailRepository>();
            services.AddScoped<IOwnedSubscriptionRepository, OwnedSubscriptionRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            return services;
        }   
    }
}
