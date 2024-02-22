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


        public User FindByUsername(string username)
		{
			User? user = _dbSet.FirstOrDefault(x => x.Username == username);
			return user;
		}

        
    }
}
