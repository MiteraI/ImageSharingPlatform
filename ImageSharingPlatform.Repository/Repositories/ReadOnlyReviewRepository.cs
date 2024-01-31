using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class ReadOnlyReviewRepository : ReadOnlyGenericRepository<Review, Guid>, IReadOnlyReviewRepository
    {
        public ReadOnlyReviewRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
