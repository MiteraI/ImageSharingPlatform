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
    public class ImageRequestService : IImageRequestService
    {
        private readonly IImageRequestRepository _imageRequestRepository;
        public ImageRequestService(IImageRequestRepository imageRequestRepository)
        {
            _imageRequestRepository = imageRequestRepository;
        }
        public async Task<ImageRequest> CreateImageRequest(ImageRequest imageRequest)
        {
            var newImageRequest = _imageRequestRepository.Add(imageRequest);
            await _imageRequestRepository.SaveChangesAsync();
            return newImageRequest;
        }

        public async Task<ImageRequest> EditImageRequest(ImageRequest imageRequest)
        {
            var editImageRequest = _imageRequestRepository.Update(imageRequest);
            await _imageRequestRepository.SaveChangesAsync();
            return editImageRequest;
        }

        public async Task<IEnumerable<ImageRequest>> GetAllImageRequestsAsync()
        {
            return await _imageRequestRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ImageRequest>> GetAllImageRequestsDetailsAsync()
        {
            return await _imageRequestRepository.GetAllWithDetailsAsync();
        }

        public async Task<ImageRequest> GetImageRequestById(Guid imageRequestId)
        {
            return await _imageRequestRepository.GetOneAsync(imageRequestId);
        }
        public async Task<bool> ImageRequestExistsAsync(Expression<Func<ImageRequest, bool>> predicate)
        {
            return await _imageRequestRepository.Exists(predicate);
        }
    }
}
