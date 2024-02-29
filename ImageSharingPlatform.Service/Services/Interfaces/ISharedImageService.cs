using ImageSharingPlatform.Domain.Entities;
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
        Task<IEnumerable<SharedImage>> GetAllSharedImagesAsync();
        Task<IEnumerable<SharedImage>> FindSharedImagesByUserIdWithFullDetails(Guid userId);
        Task<SharedImage> FindSharedImageByUserIdWithFullDetails(Guid userId);
        Task<IEnumerable<SharedImage>> FindSharedImageWithSearchNameAndCate(string searchName, Guid? imageCategoryId);
    }
}
