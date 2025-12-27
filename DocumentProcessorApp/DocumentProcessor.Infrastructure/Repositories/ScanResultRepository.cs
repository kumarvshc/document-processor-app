using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Infrastructure.Repositories
{
    public class ScanResultRepository : Repository<ScanResult>, IScanResultRepository
    {
        public ScanResultRepository(DocumentProcessorDbContext context) : base( context)
        {
        }

        public async Task<IReadOnlyCollection<ScanResult>> GetScanResultsByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(s => s.DocumentId == documentId)
                .Include(s => s.Document)
                .ToListAsync(cancellationToken);
        }
    }
}
