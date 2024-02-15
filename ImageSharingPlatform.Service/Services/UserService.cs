using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSharingPlatform.Service.Utils;

namespace ImageSharingPlatform.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public virtual async Task<User> RegisterUser(User user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            var newUser = _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }

        public User LoginUser(string username, string password)
        {
            var loginUser = _userRepository.FindByUsername(username);
            //var userList = _userRepository.GetAllAsync().Result.ToList();
            if (loginUser == null)
            {
                throw new Exception("User not found");
            }
            if (!PasswordHasher.VerifyPassword(loginUser.Password, password))
            {
                throw new Exception("Invalid password");
            }
            return new User 
            { 
                Id = loginUser.Id, 
                Roles = loginUser.Roles,
                Username = loginUser.Username, 
                AvatarUrl = loginUser.AvatarUrl
            };
        }
    }
}
