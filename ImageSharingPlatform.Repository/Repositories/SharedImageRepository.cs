﻿using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageSharingPlatform.Domain.Migrations;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;

namespace ImageSharingPlatform.Repository.Repositories
{
	public class SharedImageRepository : GenericRepository<SharedImage, Guid>, ISharedImageRepository
    {

        public SharedImageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

		public async Task<IEnumerable<SharedImage>> GetAllNonPreiumWithFullDetails()
		{
			return await _dbSet.Include(si => si.Artist).Include(si => si.ImageCategory).Where(si => !si.IsPremium).ToListAsync();
		}

        public async Task<IEnumerable<SharedImage>> GetAllPreiumWithFullDetails()
        {
            return await _dbSet.Include(si => si.Artist).Include(si => si.ImageCategory).Where(si => si.IsPremium).ToListAsync();
        }

        public async Task<SharedImage> GetSharedImageByUserIdWithFullDetails(Guid userId)
		{
			return await _dbSet
				.Include(si => si.Artist)
				.Include(si => si.ImageCategory)
				.FirstOrDefaultAsync(si => si.ArtistId == userId);
		}

		public async Task<IEnumerable<SharedImage>> GetSharedImagesByUserIdWithFullDetails(Guid userId)
		{
			return await _dbSet
				.Include(si => si.Artist)
				.Include(si => si.ImageCategory)
				.Where(si => si.ArtistId == userId)
				.ToListAsync();
		}

		public async Task<IEnumerable<SharedImage>> GetSharedImageWithSearchNameAndCateAsync(string searchName, ImageCategory? imageCategory)
		{
			if (imageCategory == null)
			{
				return await _dbSet
					.Include(si => si.Artist)
					.Include(si => si.ImageCategory)
					.Where(si => si.ImageName.Contains(searchName))
					.ToListAsync();
			}
			else
			{
				return await _dbSet
					.Include(si => si.Artist)
					.Include(si => si.ImageCategory)
					.Where(si => si.ImageName.Contains(searchName) && si.ImageCategory == imageCategory)
					.ToListAsync();
			}	
		}

		public async Task<IPage<SharedImage>> GetSharedImageWithSearchNameAndCatePageableAsync(string searchName, ImageCategory? imageCategory, IPageable pageable)
		{
			if (imageCategory == null)
			{
				return await _dbSet
					.Include(si => si.Artist)
					.Include(si => si.ImageCategory)
					.Where(si => si.ImageName.Contains(searchName) && si.IsPremium == false)
					.UsePageableAsync(pageable);
			}
			else
			{
				return await _dbSet
					.Include(si => si.Artist)
					.Include(si => si.ImageCategory)
					.Where(si => si.ImageName.Contains(searchName) && si.ImageCategory == imageCategory && si.IsPremium == false)
					.UsePageableAsync(pageable);
			}
		}
	}
}
