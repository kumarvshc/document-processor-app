using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;

namespace DocumentProcessor.Application.ServiceInterfaces
{
    public interface IDocumentService
    {
        Task<DocumentResponse> AddDocumentAsync(DocumentRequest request, CancellationToken cancellationToken = default);
        Task<DocumentStatusResponse> GetDocumentStatusAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task<DocumentTextResponse> GetDocumentTextAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task<DocumentMatchesResponse> GetDocumentMatchesAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
