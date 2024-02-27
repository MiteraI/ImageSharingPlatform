using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
	public class UserRepository : GenericRepository<User, Guid>, IUserRepository
	{

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public async Task<User> FindByUsername(string username)
		{
            return await _dbSet.Include(u => u.Roles).Where(u => u.Username == username).FirstOrDefaultAsync();   
		}

		public async Task<User> GetOneIncludeRolesAsync(Guid guid)
		{
			return await _dbSet.Include(u => u.Roles).Where(u => u.Id == guid).FirstOrDefaultAsync();
		}
	}
}
