using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Enums;
using DocumentProcessor.Domain.Interfaces;

namespace DocumentProcessor.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<DocumentResponse> AddDocumentAsync(AddDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var document = Document.Create(request.FileName, request.Content, request.MaxContextSize);

            await _unitOfWork.Documents.AddAsync(document, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DocumentResponse(
                document.Id,
                document.FileName,
                document.Status,
                document.CreatedDateTime,
                document.ProcessedDateTime
                );
        }

        public async Task<DocumentMatchesResponse> GetDocumentMatchesAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdWithScanResultsAsync(documentId, cancellationToken);

            if(document is null)
            {
                return null;
            }
            var matches = document.ScanResults.Select(s => new ScanResultResponse(
                s.Id,
                s.Position,
                s.ScanType,
                s.MatchedText,
                s.CreatedDateTime
                ));
            return new DocumentMatchesResponse(document.Id, document.FileName, matches);
        }

        public async Task<DocumentStatusResponse> GetDocumentStatusAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdWithScanResultsAsync(documentId, cancellationToken);

            if(document is null)
            {
                return new DocumentStatusResponse(documentId, DocumentStatus.Unknown);
            }

            return new DocumentStatusResponse(document.Id, document.Status);
        }

        public async Task<DocumentTextResponse> GetDocumentTextAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdWithScanResultsAsync(documentId, cancellationToken);

            if(document is null)
            {
                return null;
            }

            return new DocumentTextResponse(document.Id, document.FileName, document.Content);
        }
    }
}
