using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Repository.Repositories.Interfaces;
using ImageSharingPlatform.Service.Services.Interfaces;
using JHipsterNet.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharingPlatform.Service.Services
{
    public class TransactionService : ITransactionService
    {
        protected readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Delete(Guid id)
        {
            await _transactionRepository.DeleteByIdAsync(id);
            await _transactionRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Transaction>> FindAll()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction> FindOne(Guid id)
        {
            return await _transactionRepository.GetOneAsync(id);
        }

        public async Task<Transaction> Save(Transaction transaction)
        {
            await _transactionRepository.CreateOrUpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllForUser(Guid userId)
        {
            return await _transactionRepository.QueryHelper()
                .Filter(t => t.UserId == userId)
                .OrderBy(t => t.OrderByDescending(t => t.TransactionDate))
                .GetAllAsync();
        }
    }
}
