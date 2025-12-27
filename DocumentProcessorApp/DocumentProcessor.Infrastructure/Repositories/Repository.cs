using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DocumentProcessorDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DocumentProcessorDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }
    }
}
