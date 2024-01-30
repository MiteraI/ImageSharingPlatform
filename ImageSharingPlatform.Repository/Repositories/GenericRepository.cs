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
    public class GenericRepository<TEntity, TKey> : ReadOnlyGenericRepository<TEntity, TKey>, IGenericRepository<TEntity, TKey>, IDisposable where TEntity : BaseEntity<TKey>
    {
        public GenericRepository(IUnitOfWork context) : base(context)
        {
        }

        public virtual TEntity Add(TEntity entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public virtual bool AddRange(params TEntity[] entities)
        {
            _dbSet.AddRange(entities);
            return true;
        }

        public virtual TEntity Attach(TEntity entity)
        {
            var entry = _dbSet.Attach(entity);
            entry.State = EntityState.Added;
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public virtual TEntity Update(string id, TEntity entity)
        {
            _dbSet.Update(entity);
            return entity;
        }

        public virtual bool UpdateRange(params TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
            return true;
        }

        public async virtual Task<TEntity> CreateOrUpdateAsync(TEntity entity)
        {
            bool exists = await Exists(x => x.Id.Equals(entity.Id));
            if (entity.Id.Equals(0) && exists)
            {
                Update(entity);
            }
            else
            {
                _context.AddOrUpdateGraph(entity);
            }
            return entity;
        }

        public async virtual Task<TEntity> CreateOrUpdateAsync(TEntity entity, ICollection<Type> entitiesToBeUpdated)
        {
            bool exists = await Exists(x => x.Id.Equals(entity.Id));
            if (entity.Id.Equals(0) && exists)
            {
                Update(entity);
            }
            else
            {
                _context.AddOrUpdateGraph(entity, entitiesToBeUpdated);
            }
            return entity;
        }

        public virtual async Task Clear()
        {
            var allEntities = await _dbSet.ToListAsync();
            _dbSet.RemoveRange(allEntities);
        }

        public virtual async Task DeleteByIdAsync(TKey id)
        {
            var entity = await GetOneAsync(id);
            _dbSet.Remove(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            await Task.FromResult(_dbSet.Remove(entity));
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
