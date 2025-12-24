using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DocumentProcessor.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DocumentProcessorDbContext _context;
        private IDbContextTransaction _transaction;
        private IDocumentRepository _documentRepository;
        private IScanResultRepository _scanResultRepository;

        public UnitOfWork(DocumentProcessorDbContext context)
        {
            _context = context;
        }

        public IDocumentRepository Documents => _documentRepository ??= new DocumentRepository(_context);
        public IScanResultRepository SacnResults => _scanResultRepository ??= new ScanResultRepository(_context);

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if(_transaction is not null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
