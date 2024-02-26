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
	public class RoleRepository : GenericRepository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork context) : base(context)
        {
        }

		public async Task<Role> GetRoleByNameAsync(UserRole userRole)
		{
			return await _dbSet.FirstOrDefaultAsync(ur => ur.UserRole == userRole);
		}
	}
}
