namespace DocumentProcessor.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentRepository Documents { get; }
        IScanResultRepository ScanResults { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
