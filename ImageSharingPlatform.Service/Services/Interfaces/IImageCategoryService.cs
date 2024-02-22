using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface IImageCategoryService
    {
        Task<ImageCategory> CreateImageCategory(ImageCategory imageCategory);
        Task<ImageCategory> EditImageCategory(ImageCategory imageCategory);
        Task<ImageCategory> DeleteImageCategory(Guid imageCategoryId);
        Task<ImageCategory> GetImageCategoryByIdAsync(Guid imageCategoryId);
        Task<bool> ImageCategoryExistsAsync(Expression<Func<ImageCategory, bool>> predicate);
        Task<IEnumerable<ImageCategory>> GetAllImageCategoriesAsync();
    }
}
