using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Application.ServiceInterfaces
{
    public interface IServiceBusMessageService
    {
        Task PublishDocumentCreatedAsync(Document document, CancellationToken cancellationToken = default);
        Task PublishScanCompletedAsync(Guid documentId, string content, CancellationToken cancellationToken = default);
    }
}
