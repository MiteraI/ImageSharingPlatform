using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Domain.Enums;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class RequestDetailRepository : GenericRepository<RequestDetail, Guid>, IRequestDetailRepository
    {
        public RequestDetailRepository(IUnitOfWork context) : base(context)
        {
        }

        public async Task<IEnumerable<RequestDetail>> GetAllByRequestId(Guid requestId)
        {
            return await _dbSet.Include(x => x.User).Where(x => x.RequestId == requestId).ToListAsync();
        }
    }
}
