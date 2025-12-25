using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Common;

namespace DocumentProcessor.Application.ServiceInterfaces
{
    public interface IDocumentService
    {
        Task<Result<DocumentResponse>> AddDocumentAsync(AddDocumentRequest request, CancellationToken cancellationToken = default);
        Task<Result<DocumentStatusResponse>> GetDocumentStatusAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task<Result<DocumentTextResponse>> GetDocumentTextAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task<Result<DocumentMatchesResponse>> GetDocumentMatchesAsync(Guid documentId, CancellationToken cancellationToken = default);
    }
}
