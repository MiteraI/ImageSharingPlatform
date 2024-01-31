using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class ImageRequestRepository : GenericRepository<ImageRequest, Guid>, IImageRequestRepository
    {
        public ImageRequestRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
