using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Infrastructure.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository( DocumentProcessorDbContext context) : base(context)
        {                
        }

        public async Task<Document> GetAllByFileNameAsync(string fileName, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.FileName == fileName, cancellationToken);
        }

        public async Task<Document> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Include(d => d.ScanResults).FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }
    }
}
