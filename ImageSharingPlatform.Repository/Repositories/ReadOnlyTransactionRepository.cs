using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Repository.Repositories
{
    public class ReadOnlyTransactionRepository : ReadOnlyGenericRepository<Transaction, Guid>, IReadOnlyTransactionRepository
    {
        public ReadOnlyTransactionRepository(IUnitOfWork context) : base(context)
        {
        }
    }
}
