using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using JHipsterNet.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services
{
	public class SubscriptionPackageService : ISubscriptionPackageService
    {

		private readonly ISubscriptionPackageRepository _subscriptionPackageRepository;

		public SubscriptionPackageService(ISubscriptionPackageRepository subscriptionPackageRepository)
		{
			_subscriptionPackageRepository = subscriptionPackageRepository;
		}

        public async Task<SubscriptionPackage> CreateSubscriptionPackage(SubscriptionPackage subscriptionPackage)
        {
            var newSubscriptionPackage = _subscriptionPackageRepository.Add(subscriptionPackage);
            await _subscriptionPackageRepository.SaveChangesAsync();
            return newSubscriptionPackage;
        }

        public async Task<SubscriptionPackage> DeleteSubscriptionPackage(Guid subscriptionPackageId)
        {
            var subcriptionPackage = await _subscriptionPackageRepository.GetOneAsync(subscriptionPackageId);
            if (subcriptionPackage != null)
            {
                await _subscriptionPackageRepository.DeleteAsync(subcriptionPackage);
                await _subscriptionPackageRepository.SaveChangesAsync();
                return subcriptionPackage;
            }
            throw new Exception("Subcription Package is not found");
        }

        public async Task<SubscriptionPackage> EditSubscriptionPackage(SubscriptionPackage subscriptionPackage)
        {
            var editSubscriptionPackage = _subscriptionPackageRepository.Update(subscriptionPackage);
            await _subscriptionPackageRepository.SaveChangesAsync();
            return editSubscriptionPackage;
        }

        public async Task<IEnumerable<SubscriptionPackage>> GetSubscriptionPackagesByArtistIdWithFullDetails(Guid userId)
        {   
            return await _subscriptionPackageRepository.GetAllByArtistIdAsync(userId);
        }

        public async Task<SubscriptionPackage> GetSubscriptionPackageByArtistId(Guid artistId)
        {
            return await _subscriptionPackageRepository.GetByArtistIdAsync(artistId);
        }

        public async Task<SubscriptionPackage> GetSubscriptionPackageById(Guid subscriptionPackageId)
        {
            return await _subscriptionPackageRepository.GetOneAsync(subscriptionPackageId);
        }

        public async Task<bool> SubscriptionPackageExistsAsync(Expression<Func<SubscriptionPackage, bool>> predicate)
        {   
            return await _subscriptionPackageRepository.Exists(predicate);
        }

        public async Task<IEnumerable<SubscriptionPackage>> GetAllSubscriptionsAsync()
        {
            return await _subscriptionPackageRepository.GetAllArtistAsync();
        }
    }
}
