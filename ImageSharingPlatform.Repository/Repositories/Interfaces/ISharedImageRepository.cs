using ImageSharingPlatform.Domain.Entities;
using JHipsterNet.Core.Pagination;
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
        Task<IEnumerable<SharedImage>> GetAllNonPreiumWithFullDetails();
        Task<IEnumerable<SharedImage>> GetAllPreiumWithFullDetails();
        Task<IEnumerable<SharedImage>> GetSharedImageWithSearchNameAndCateAsync(string searchName, ImageCategory? imageCategory);
		Task<IPage<SharedImage>> GetSharedImageWithSearchNameAndCatePageableAsync(string searchName, ImageCategory? imageCategory, IPageable pageable);

	}
}
