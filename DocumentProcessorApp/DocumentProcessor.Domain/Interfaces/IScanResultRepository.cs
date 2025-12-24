using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Domain.Interfaces
{
    public interface IScanResultRepository
    {
        Task<ScanResult> GeDocumentScanStatusAsync(Guid documentId, CancellationToken cancellation = default);
    }
}
