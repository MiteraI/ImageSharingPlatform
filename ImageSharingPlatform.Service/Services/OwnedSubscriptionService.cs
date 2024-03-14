using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services
{
    public class OwnedSubscriptionService : IOwnedSubscriptionService
    {
        private readonly IOwnedSubscriptionRepository _ownedSubscriptionRepository;

        public OwnedSubscriptionService(IOwnedSubscriptionRepository ownedSubscriptionRepository)
        {
            _ownedSubscriptionRepository = ownedSubscriptionRepository;
        }

        public async Task<OwnedSubscription> CreateOwnedSubscription(OwnedSubscription ownedSubscription)
        {
            var newOwnedSubscription = _ownedSubscriptionRepository.Add(ownedSubscription);
            await _ownedSubscriptionRepository.SaveChangesAsync();
            return newOwnedSubscription;
        }

        public async Task<IEnumerable<OwnedSubscription>> GetAllOwnedSubscriptionsAsync()
        {
            return await _ownedSubscriptionRepository.GetAllOwnedSubscriptionPackageAsync();
        }

        public async Task<OwnedSubscription> GetOwnedSubscriptionPackage(Guid? packageId)
        {
            return await _ownedSubscriptionRepository.GetBySubscriptionPackageIdAsync(packageId);
        }

        public async Task renewSubscription(Guid? packageId)
        {
            var existingSubscriptionPackage = await _ownedSubscriptionRepository.GetBySubscriptionPackageIdAsync(packageId);
            if (existingSubscriptionPackage != null)
            {
                if (DateTime.Now > existingSubscriptionPackage.PurchasedTime.AddDays(30))
                {
                    existingSubscriptionPackage.PurchasedTime = DateTime.Now;
                    await _ownedSubscriptionRepository.SaveChangesAsync();
                }
            }
        }
    }
}
