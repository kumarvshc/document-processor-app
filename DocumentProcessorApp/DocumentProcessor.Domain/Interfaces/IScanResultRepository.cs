using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Domain.Interfaces
{
    public interface IScanResultRepository : IRepository<ScanResult>
    {
        Task<IEnumerable<ScanResult>> GeDocumentScanStatusAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
