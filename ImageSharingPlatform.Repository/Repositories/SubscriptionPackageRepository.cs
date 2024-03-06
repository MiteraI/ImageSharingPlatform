using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Enums;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class SubscriptionPackageRepository : GenericRepository<SubscriptionPackage, Guid>, ISubscriptionPackageRepository
    {
        public SubscriptionPackageRepository(IUnitOfWork context) : base(context)
        {
            
        }

		public async Task<IEnumerable<SubscriptionPackage>> GetAllByArtistIdAsync(Guid userId)
        {
            return await _dbSet.Include(ir => ir.Artist).Where(ir => ir.ArtistId == userId).ToListAsync();

        }

        public async Task<SubscriptionPackage> GetByArtistIdAsync(Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(sp => sp.ArtistId == userId); ;
        }
    }
}
