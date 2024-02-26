using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUser(User user);

        Task<User> LoginUser(string username, string password);

        Task UpdateRoleToArtist(Guid userId);

        Task<User> CreateUser(User user);
        Task<User> EditUser(User user);
        Task<User> DeleteUser(Guid userId);

        Task<User> GetUserByIdAsync(Guid userId);
        Task<bool> UserExistsAsync(Expression<Func<User, bool>> predicate);
        Task<IEnumerable<User>> GetAllUsersAsync();

    }
}
