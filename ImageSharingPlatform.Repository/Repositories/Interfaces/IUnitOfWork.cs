using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbSet<T> Set<T>(string name = null) where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        void AddOrUpdateGraph<TEntiy>(TEntiy entity, ICollection<Type> entitiesToBeUpdated = null) where TEntiy : class;
        void UpdateState<TEntity>(TEntity entity, EntityState state);
        void SetEntityStateModified<TEntiy, TProperty>(TEntiy entity, Expression<Func<TEntiy, TProperty>> propertyExpression) where TEntiy : class where TProperty : class;
    }
}
