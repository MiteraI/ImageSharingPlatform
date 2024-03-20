using ImageSharingPlatform.Domain.Entities;
using JHipsterNet.Core.Pagination;
using PartyRentingPlatform.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface IReadOnlyGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetOneAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IPage<TEntity>> GetPageAsync(IPageable pageable);
        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        IFluentRepository<TEntity> QueryHelper();
    }
}
