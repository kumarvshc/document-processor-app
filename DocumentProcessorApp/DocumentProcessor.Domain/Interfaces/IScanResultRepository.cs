using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Domain.Interfaces
{
    public interface IScanResultRepository
    {
        Task<IEnumerable<ScanResult>> GeDocumentScanStatusAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
