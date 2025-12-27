using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Domain.Interfaces
{
    public interface IScanResultRepository : IRepository<ScanResult>
    {
        Task<IReadOnlyCollection<ScanResult>> GetScanResultsByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
