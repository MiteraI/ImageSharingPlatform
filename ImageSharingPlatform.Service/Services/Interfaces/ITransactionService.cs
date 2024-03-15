using ImageSharingPlatform.Domain.Entities;
using JHipsterNet.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> Save(Transaction Transaction);

        Task<IEnumerable<Transaction>> FindAll();

        Task<Transaction> FindOne(Guid id);

        Task Delete(Guid id);
        Task<IEnumerable<Transaction>> GetAllForUser(Guid userId);

	}
}
