using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services
{
    public class ImageCategoryService : IImageCategoryService
    {

        private readonly IImageCategoryRepository _imageCategoryRepository;
        public ImageCategoryService(IImageCategoryRepository imageCategoryRepository)
        {
            _imageCategoryRepository = imageCategoryRepository;
        }


        public async Task<ImageCategory> CreateImageCategory(ImageCategory imageCategory)
        {
            var newImageCategory = _imageCategoryRepository.Add(imageCategory);
            await _imageCategoryRepository.SaveChangesAsync();
            return newImageCategory;
        }

        public async Task<ImageCategory> EditImageCategory(ImageCategory imageCategory)
        {
            var newImageCategory = _imageCategoryRepository.Update(imageCategory);
            await _imageCategoryRepository.SaveChangesAsync();
            return newImageCategory;
        }

        public async Task<ImageCategory> DeleteImageCategory(Guid imageCategoryId)
        {
            var newImageCategory = await _imageCategoryRepository.GetOneAsync(imageCategoryId);
            if (newImageCategory != null)
            {
                _imageCategoryRepository.DeleteAsync(newImageCategory);
                await _imageCategoryRepository.SaveChangesAsync();
                return newImageCategory;
            }
            throw new Exception("ImageCategory not found");
        }

        public async Task<ImageCategory> GetImageCategoryByIdAsync(Guid imageCategoryId)
        {
            return await _imageCategoryRepository.GetOneAsync(imageCategoryId);
        }

        public async Task<bool> ImageCategoryExistsAsync(Expression<Func<ImageCategory, bool>> predicate)
        {
            return await _imageCategoryRepository.Exists(predicate);
        }

        public async Task<IEnumerable<ImageCategory>> GetAllImageCategoriesAsync()
        {
            return await _imageCategoryRepository.GetAllAsync();
        }
    }
}
