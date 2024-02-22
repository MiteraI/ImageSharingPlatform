using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUser(User user);

        Task<User> LoginUser(string username, string password);
    }
}
