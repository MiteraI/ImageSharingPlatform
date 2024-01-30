using ImageSharingPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> : IReadOnlyGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> CreateOrUpdateAsync(TEntity entity);
        Task<TEntity> CreateOrUpdateAsync(TEntity entity, ICollection<Type> entitiesToBeUpdated);
        Task DeleteByIdAsync(TKey id);
        Task DeleteAsync(TEntity entity);
        Task Clear();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        TEntity Add(TEntity entity);
        bool AddRange(params TEntity[] entities);
        TEntity Attach(TEntity entity);
        TEntity Update(TEntity entity);
        bool UpdateRange(params TEntity[] entities);
    }
}
