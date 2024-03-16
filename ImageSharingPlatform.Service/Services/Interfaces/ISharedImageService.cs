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
    public interface ISharedImageService
    {
        Task<SharedImage> CreateSharedImage(SharedImage sharedImage);
        Task<SharedImage> EditSharedImage(SharedImage sharedImage);
        Task<SharedImage> DeleteSharedImage(Guid sharedImageId);
        Task<SharedImage> GetSharedImageByIdAsync(Guid sharedImageId);
        Task<bool> SharedImageExistsAsync(Expression<Func<SharedImage, bool>> predicate);
        Task<IEnumerable<SharedImage>> GetAllNonPremiumSharedImagesAsync();
        Task<IEnumerable<SharedImage>> GetAllPremiumSharedImagesAsync();
        Task<IEnumerable<SharedImage>> FindSharedImagesByUserIdWithFullDetails(Guid userId);
        Task<SharedImage> FindSharedImageByUserIdWithFullDetails(Guid userId);
        Task<IPage<SharedImage>> FindSharedImageWithSearchNameAndCatePageable(string searchName, Guid? imageCategoryId, IPageable pageable);
        Task<IEnumerable<SharedImage>> FindSharedImageByArtistId(Guid artistId, bool isPremium);
        Task<Review> CreateReviewForImage(Guid sharedImageId, Review review);
        Task<IPage<SharedImage>> FindSharedImageForArtistPageable(Guid artist, bool? isPremium, IPageable pageable);
    }
}
