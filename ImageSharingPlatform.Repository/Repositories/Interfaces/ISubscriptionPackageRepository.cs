using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface ISubscriptionPackageRepository : IGenericRepository<SubscriptionPackage, Guid>
    {
		Task<IEnumerable<SubscriptionPackage>> GetAllByArtistIdAsync(Guid userId);
        Task<IEnumerable<SubscriptionPackage>> GetAllArtistAsync();
        Task<SubscriptionPackage> GetByArtistIdAsync(Guid userId);
    }
}
