using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Common;
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
        
        public async Task<Result<DocumentResponse>> AddDocumentAsync(AddDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var document = Document.Create(request.FileName, request.Content, request.MaxContextSize);

            await _unitOfWork.Documents.AddAsync(document, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<DocumentResponse>.Success(new DocumentResponse(
                document.Id,
                document.FileName,
                document.Status,
                document.CreatedDateTime,
                document.ProcessedDateTime
                ));
        }

        public async Task<Result<DocumentMatchesResponse>> GetDocumentMatchesAsync(Guid documentId, CancellationToken cancellationToken = default)
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
            return Result<DocumentMatchesResponse>.Success(new DocumentMatchesResponse(document.Id, document.FileName, matches));
        }

        public async Task<Result<DocumentStatusResponse>> GetDocumentStatusAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdWithScanResultsAsync(documentId, cancellationToken);

            if(document is null)
            {
                return Result<DocumentStatusResponse>.Success(new DocumentStatusResponse(documentId, DocumentStatus.Unknown));
            }

            return Result<DocumentStatusResponse>.Success(new DocumentStatusResponse(document.Id, document.Status));
        }

        public async Task<Result<DocumentTextResponse>> GetDocumentTextAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdWithScanResultsAsync(documentId, cancellationToken);

            if(document is null)
            {
                return null;
            }

            return Result<DocumentTextResponse>.Success(new DocumentTextResponse(document.Id, document.FileName, document.Content));
        }
    }
}
