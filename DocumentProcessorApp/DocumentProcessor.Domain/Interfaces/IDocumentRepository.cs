

using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Domain.Interfaces
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<Document> GetByIdWithScanResultsAsync(Guid id, CancellationToken cancellation = default);        
        Task<Document> GetAllByFileNameAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
