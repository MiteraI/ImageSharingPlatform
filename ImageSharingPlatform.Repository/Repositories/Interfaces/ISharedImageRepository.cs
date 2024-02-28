using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface ISharedImageRepository : IGenericRepository<SharedImage, Guid>
    {
        Task<IEnumerable<SharedImage>> GetSharedImagesByUserIdWithFullDetails(Guid userId);
        Task<SharedImage> GetSharedImageByUserIdWithFullDetails(Guid userId);
    }
}
