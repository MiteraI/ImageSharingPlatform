using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Migrations;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class ImageCategoryRepository : GenericRepository<ImageCategory, Guid>, IImageCategoryRepository
    {

        public ImageCategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
