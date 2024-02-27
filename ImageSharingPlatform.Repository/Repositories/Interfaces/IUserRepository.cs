using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        Task<User> FindByUsername(string username);
        Task<User> GetOneIncludeRolesAsync(Guid guid);
    }
}
