using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSharingPlatform.Domain.Migrations;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class SharedImageRepository : GenericRepository<SharedImage, Guid>, ISharedImageRepository
    {

        public SharedImageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

    }
}
