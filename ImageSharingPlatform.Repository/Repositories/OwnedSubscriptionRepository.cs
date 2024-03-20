using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class OwnedSubscriptionRepository : GenericRepository<OwnedSubscription, Guid>, IOwnedSubscriptionRepository
    {
        public OwnedSubscriptionRepository(IUnitOfWork context) : base(context)
        {

        }

        public async Task<IEnumerable<OwnedSubscription>> GetAllOwnedSubscriptionPackageAsync()
        {
            return await _dbSet.Include(si => si.SubscriptionPackage).Include(si => si.User).ToListAsync();
        }

        public async Task<OwnedSubscription> GetBySubscriptionPackageIdAsync(Guid? packageId)
        {
            return await _dbSet.FirstOrDefaultAsync(sp => sp.SubscriptionPackageId == packageId);
        }
    }
}
