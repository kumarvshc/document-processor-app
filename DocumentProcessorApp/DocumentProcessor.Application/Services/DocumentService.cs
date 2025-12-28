using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Common;
using DocumentProcessor.Domain.Interfaces;

namespace DocumentProcessor.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;

        public DocumentService(IUnitOfWork unitOfWork, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
        }

        public async Task<Result<DocumentResponse>> AddDocumentAsync(AddDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var document = Domain.Entities.Document.Create(request.FileName, request.Content, request.Metadata, request.MaxContentSize);

            await _unitOfWork.Documents.AddAsync(document, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _messageService.PublishDocumentCreatedAsync(document, cancellationToken);

            return Result<DocumentResponse>.Success(new DocumentResponse(
                document.Id,
                document.FileName,
                document.Status,
                document.CreatedDateTime
                ));
        }

        public async Task<Result<DocumentMatchesResponse>> GetDocumentMatchesAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var scanResults = await _unitOfWork.ScanResults.GetScanResultsByDocumentIdAsync(documentId, cancellationToken);

            if (!scanResults.Any())
            {
                return Result<DocumentMatchesResponse>.NotFound("Cannot find scan results for the given document id.");
            }

            var firstResult = scanResults.First();

            var matches = scanResults.Select(s => new ScanResultResponse(
                s.Id,
                s.Position,
                s.ScanType,
                s.MatchedText,
                s.CreatedDateTime
                ));

            return Result<DocumentMatchesResponse>.Success(new DocumentMatchesResponse(
                firstResult.Document.Id,
                firstResult.Document.FileName,
                firstResult.Document.ProcessedDateTime,
                matches));
        }

        public async Task<Result<DocumentStatusResponse>> GetDocumentStatusAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdAsync(documentId, cancellationToken);

            if (document is null)
            {
                return Result<DocumentStatusResponse>.NotFound("Cannot find document for the given document id.");
            }

            return Result<DocumentStatusResponse>.Success(new DocumentStatusResponse(document.Id, document.Status));
        }

        public async Task<Result<DocumentTextResponse>> GetDocumentTextAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdAsync(documentId, cancellationToken);

            if (document is null)
            {
                return Result<DocumentTextResponse>.NotFound("Cannot find document for the given document id.");
            }

            return Result<DocumentTextResponse>.Success(new DocumentTextResponse(document.Id, document.FileName, document.Content));
        }
    }
}
