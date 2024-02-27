using ImageSharingPlatform.Configuration;

namespace ImageSharingPlatform
{
    public class Startup
    {
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDatabaseModule(configuration)
                .AddReposiotoryModule()
                .AddServiceModule()
                .AddAzureBlobModule()
                .AddVnpayModule()
                .AddAutoMapper(typeof(Startup));
        }
    }
}
