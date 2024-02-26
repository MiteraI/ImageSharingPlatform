using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSharingPlatform.Service.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static System.Net.WebRequestMethods;
using ImageSharingPlatform.Domain.Enums;

namespace ImageSharingPlatform.Service.Services
{
	public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public virtual async Task<User> RegisterUser(User user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            user.AvatarUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50";
            var userRole = await _roleRepository.GetRoleByNameAsync(UserRole.ROLE_USER);
            user.Roles.Add(userRole);
            var newUser = _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> LoginUser(string username, string password)
        {
            var loginUser = await _userRepository.FindByUsername(username);
            if (loginUser == null)
            {
                throw new Exception("User not found");
            }
            if (!PasswordHasher.VerifyPassword(loginUser.Password, password))
            {
                throw new Exception("Invalid password");
            }
            loginUser.Password = null;
            return loginUser;
        }

        public async Task<User> CreateUser(User user)
        {
            var newUser = _userRepository.Add(user);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> EditUser(User user)
        {
            var newUser = _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            return newUser;
        }

        public async Task<User> DeleteUser(Guid userId)
        {
            var user = await _userRepository.GetOneAsync(userId);
            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
                await _userRepository.SaveChangesAsync();
                return user;
            }
            throw new Exception("User not found");
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _userRepository.GetOneAsync(userId);
        }

        public async Task<bool> UserExistsAsync(Expression<Func<User, bool>> predicate)
        {
            return await _userRepository.Exists(predicate);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

		public async Task UpdateRoleToArtist(Guid userId)
		{
            User user = await _userRepository.GetOneIncludeRolesAsync(userId);
			Role artistRole = await _roleRepository.GetRoleByNameAsync(UserRole.ROLE_ARTIST);

            // Check if user already has the artist role
            if (user.Roles.Any(r => r.UserRole == artistRole.UserRole))
            {
				return;
			}

            user.Roles.Add(artistRole);
			_userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
		}
	}
}
