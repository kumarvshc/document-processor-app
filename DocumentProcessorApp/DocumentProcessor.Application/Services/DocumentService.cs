using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Common;
using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Interfaces;
using System.Reflection.Metadata;

namespace DocumentProcessor.Application.Services
{
    public class DocumentService : IDocumentService, IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _messagePublisher;

        public DocumentService(IUnitOfWork unitOfWork, IMessagePublisher messagePublisher)
        {
            _unitOfWork = unitOfWork;
            _messagePublisher = messagePublisher;
        }

        public async Task<Result<DocumentResponse>> AddDocumentAsync(AddDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var document = Domain.Entities.Document.Create(request.FileName, request.Content, request.Metadata, request.MaxContentSize);

            await _unitOfWork.Documents.AddAsync(document, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await PublishDocumentCreatedAsync(document, cancellationToken);

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

            if (document is null)
            {
                return Result<DocumentMatchesResponse>.Failure("Cannot find document for the given document id.");
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

            if (document is null)
            {
                return Result<DocumentStatusResponse>.Failure("Cannot find document for the given document id.");
            }

            return Result<DocumentStatusResponse>.Success(new DocumentStatusResponse(document.Id, document.Status));
        }

        public async Task<Result<DocumentTextResponse>> GetDocumentTextAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            var document = await _unitOfWork.Documents.GetByIdWithScanResultsAsync(documentId, cancellationToken);

            if (document is null)
            {
                return Result<DocumentTextResponse>.Failure("Cannot find document for the given document id.");
            }

            return Result<DocumentTextResponse>.Success(new DocumentTextResponse(document.Id, document.FileName, document.Content));
        }

        public async Task PublishDocumentCreatedAsync(Domain.Entities.Document document, CancellationToken cancellationToken = default)
        {
            await _messagePublisher.PublishDocumentCreatedAsync(document.Id, document.FileName, document.Content, cancellationToken);
        }
    }
}
