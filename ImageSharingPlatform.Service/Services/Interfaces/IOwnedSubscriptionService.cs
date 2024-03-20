using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface IOwnedSubscriptionService
    {
        Task<OwnedSubscription> CreateOwnedSubscription(OwnedSubscription ownedSubscription);
        Task<IEnumerable<OwnedSubscription>> GetAllOwnedSubscriptionsAsync();
        Task<OwnedSubscription> GetOwnedSubscriptionPackage(Guid? packageId);
        Task renewSubscription(Guid? packageId);
        Task<OwnedSubscription> GetUserOwnedSubscriptionPackage(Guid? userId, Guid? packageId);
        Task<IEnumerable<OwnedSubscription>> GetAllOwnedSubscriptionForUser(Guid userId);
    }
}
