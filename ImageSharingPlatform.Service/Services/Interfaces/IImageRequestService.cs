using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface IImageRequestService
    {
        Task<ImageRequest> CreateImageRequest(ImageRequest imageRequest);
        Task<ImageRequest> GetImageRequestByIdWithFullDetailsAsync(Guid imageRequestId);
        Task<ImageRequest> EditImageRequest(ImageRequest imageRequest);
        Task<bool> ImageRequestExistsAsync(Expression<Func<ImageRequest, bool>> predicate);
        Task<IEnumerable<ImageRequest>> GetAllImageRequestsAsync();
		Task<IEnumerable<ImageRequest>> GetAllImageRequestsByUserAsync(Guid userId);
        Task<IEnumerable<ImageRequest>> GetAllImageRequestsByArtistAsync(Guid artistId);
        Task<IEnumerable<ImageRequest>> GetAllImageRequestsDetailsAsync();

    }
}
