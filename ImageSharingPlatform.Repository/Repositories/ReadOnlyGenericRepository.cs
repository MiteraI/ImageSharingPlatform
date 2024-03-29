﻿using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using Microsoft.EntityFrameworkCore;
using PartyRentingPlatform.Domain.Repositories.Interfaces;
using PartyRentingPlatform.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class ReadOnlyGenericRepository<TEntity, TKey> : IReadOnlyGenericRepository<TEntity, TKey>, IDisposable where TEntity : BaseEntity<TKey>
    {
        protected internal readonly IUnitOfWork _context;
        protected internal readonly DbSet<TEntity> _dbSet;

        public ReadOnlyGenericRepository(IUnitOfWork context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetOneAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IPage<TEntity>> GetPageAsync(IPageable pageable)
        {
            return await _dbSet.UsePageableAsync(pageable);
        }

        public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync()
        {
            var countTask = await _dbSet.CountAsync();
            return countTask;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public IFluentRepository<TEntity> QueryHelper()
        {
            var fluentRepository = new FluentRepository<TEntity>(_dbSet);
            return fluentRepository;
        }
    }
}
