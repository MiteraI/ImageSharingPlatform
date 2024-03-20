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
    public class ImageRequestRepository : GenericRepository<ImageRequest, Guid>, IImageRequestRepository
    {
        public ImageRequestRepository(IUnitOfWork context) : base(context)
        {
            
        }

        public async Task<IEnumerable<ImageRequest>> GetAllWithDetailsAsync()
        {
            return await _dbSet.Include(ir => ir.RequesterUser)
           .Include(ir => ir.Artist).ToListAsync();
        }

		public async Task<IEnumerable<ImageRequest>> GetAllByUserIdAsync(Guid userId)
        {
            return await _dbSet.Include(ir => ir.RequesterUser)
           .Include(ir => ir.Artist).Where(ir => ir.RequesterUserId == userId).ToListAsync();

        }

        public async Task<IEnumerable<ImageRequest>> GetAllByArtistIdAsync(Guid artistId)
        {
            return await _dbSet.Include(ir => ir.RequesterUser)
           .Include(ir => ir.Artist).Where(ir => ir.ArtistId == artistId).ToListAsync();
        }

        public async Task<ImageRequest> GetByIdWithFullDetails(Guid id)
        {
            return await _dbSet.Include(si => si.Artist).Include(si => si.RequesterUser)
                .FirstOrDefaultAsync(si => si.Id == id);
        }
    }
}
