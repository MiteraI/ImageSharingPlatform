using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class ReadOnlyRoleRepository : ReadOnlyGenericRepository<Role, Guid>, IReadOnlyRoleRepository
    {
        public ReadOnlyRoleRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
