

using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetByIdAsync(Guid id, CancellationToken cancellation = default);        
        Task<Document> GetAllByFileNameAsync(string fileName, CancellationToken cancellationToken = default);
    }
}
