using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface IImageRequestRepository : IGenericRepository<ImageRequest, Guid>
    {
        Task<IEnumerable<ImageRequest>> GetAllWithDetailsAsync();
		Task<IList<ImageRequest>> GetByUserIdAsync(Guid userId);
	}
}
