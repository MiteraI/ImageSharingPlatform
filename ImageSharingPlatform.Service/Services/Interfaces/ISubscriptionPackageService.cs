using ImageSharingPlatform.Domain.Entities;
using JHipsterNet.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface ISubscriptionPackageService
    {
        Task<SubscriptionPackage> CreateSubscriptionPackage(SubscriptionPackage subscriptionPackage);
        Task<SubscriptionPackage> EditSubscriptionPackage(SubscriptionPackage subscriptionPackage);
        Task<SubscriptionPackage> DeleteSubscriptionPackage(Guid subscriptionPackageId);
        Task<bool> SubscriptionPackageExistsAsync(Expression<Func<SubscriptionPackage, bool>> predicate);
        Task<IEnumerable<SubscriptionPackage>> GetAllSubscriptionsAsync();
        Task<IEnumerable<SubscriptionPackage>> GetSubscriptionPackagesByArtistIdWithFullDetails(Guid userId);
        Task<SubscriptionPackage> GetSubscriptionPackageById(Guid subscriptionPackageId);
        Task<SubscriptionPackage> GetSubscriptionPackageByArtistId(Guid artistId);
    }
}
