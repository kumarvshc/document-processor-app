using System.Reflection.Metadata;

namespace DocumentProcessor.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> GetDocumentStatusByIdAsync(Guid id, CancellationToken cancellation = default);        
        Task<Document> GetDocumentContentyByFileNameAsync(string fileName, CancellationToken cancellation = default);
    }
}
