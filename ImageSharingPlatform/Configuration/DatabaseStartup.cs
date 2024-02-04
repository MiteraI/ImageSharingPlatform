using ImageSharingPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageSharingPlatform.Configuration
{
    public static class DatabaseStartup
    {
        public static IServiceCollection AddDatabaseModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionStringDB")));

            services.AddScoped<DbContext>(options => options.GetService<ApplicationDbContext>());

            return services;
        }
    }
}
