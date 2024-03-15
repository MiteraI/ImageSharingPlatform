using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using JHipsterNet.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services
{
	public class SharedImageService : ISharedImageService
	{

		private readonly ISharedImageRepository _sharedImageRepository;
		private readonly IImageCategoryRepository _imageCategoryRepository;

		public SharedImageService(ISharedImageRepository sharedImageRepository, IImageCategoryRepository imageCategoryRepository)
		{
			_sharedImageRepository = sharedImageRepository;
			_imageCategoryRepository = imageCategoryRepository;
		}

		public async Task<SharedImage> CreateSharedImage(SharedImage sharedImage)
		{
			var newSharedImage = _sharedImageRepository.Add(sharedImage);
			await _sharedImageRepository.SaveChangesAsync();
			return newSharedImage;
		}

		public async Task<SharedImage> EditSharedImage(SharedImage sharedImage)
		{
			var newSharedImage = _sharedImageRepository.Update(sharedImage);
			await _sharedImageRepository.SaveChangesAsync();
			return newSharedImage;
		}

		public async Task<SharedImage> DeleteSharedImage(Guid sharedImageId)
		{
			var newSharedImage = await _sharedImageRepository.GetOneAsync(sharedImageId);
			if (newSharedImage != null)
			{
				await _sharedImageRepository.DeleteAsync(newSharedImage);
				await _sharedImageRepository.SaveChangesAsync();
				return newSharedImage;
			}
			throw new Exception("SharedImage not found");
		}

		public async Task<SharedImage> GetSharedImageByIdAsync(Guid sharedImageId)
		{
			return await _sharedImageRepository.GetOneAsync(sharedImageId);
		}

		public async Task<bool> SharedImageExistsAsync(Expression<Func<SharedImage, bool>> predicate)
		{
			return await _sharedImageRepository.Exists(predicate);
		}

		public async Task<IEnumerable<SharedImage>> GetAllNonPremiumSharedImagesAsync()
		{
			return await _sharedImageRepository.GetAllNonPreiumWithFullDetails();
		}

		public async Task<SharedImage> FindSharedImageByUserIdWithFullDetails(Guid userId)
		{
			return await _sharedImageRepository.GetSharedImageByUserIdWithFullDetails(userId);
		}

		public async Task<IEnumerable<SharedImage>> FindSharedImagesByUserIdWithFullDetails(Guid userId)
		{
			return await _sharedImageRepository.GetSharedImagesByUserIdWithFullDetails(userId);
		}

		public async Task<IEnumerable<SharedImage>> FindSharedImageWithSearchNameAndCate(string searchName, Guid? imageCategoryId)
		{
			if (imageCategoryId == null)
			{
				return await _sharedImageRepository.GetSharedImageWithSearchNameAndCateAsync(searchName, null);
			} 

			var category = await _imageCategoryRepository.GetOneAsync(imageCategoryId.Value);
			return await _sharedImageRepository.GetSharedImageWithSearchNameAndCateAsync(searchName, category);
		}

		public async Task<IPage<SharedImage>> FindSharedImageWithSearchNameAndCatePageable(string searchName, Guid? imageCategoryId, IPageable pageable)
		{
			if (imageCategoryId == null)
			{
				return await _sharedImageRepository.QueryHelper().Filter(si => si.ImageName.Contains(searchName)).GetPageAsync(pageable);
			}

			var category = await _imageCategoryRepository.GetOneAsync(imageCategoryId.Value);
			return await _sharedImageRepository.QueryHelper().Filter(si => si.ImageName.Contains(searchName) && si.ImageCategory == category).GetPageAsync(pageable);
		}

        public async Task<IEnumerable<SharedImage>> GetAllPremiumSharedImagesAsync()
        {
            return await _sharedImageRepository.GetAllPreiumWithFullDetails();
        }
    }
}
