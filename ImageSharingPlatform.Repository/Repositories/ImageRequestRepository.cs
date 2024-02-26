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
    }
}
