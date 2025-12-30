using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Interfaces;

namespace DocumentProcessor.Application.Services
{
    public class ServiceBusMessageService : IServiceBusMessageService
    {
        private readonly IServiceBusMessagePublisher _messagePublisher;

        public ServiceBusMessageService(IServiceBusMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public async Task PublishDocumentCreatedAsync(Document document, CancellationToken cancellationToken = default)
        {
            await _messagePublisher.PublishDocumentCreatedAsync(document.Id, document.Content, cancellationToken);
        }

        public async Task PublishScanCompletedAsync(Guid documentId, string content, CancellationToken cancellationToken = default)
        {
            await _messagePublisher.PublishScanCompletedAsync(documentId, content, cancellationToken);
        }
    }
}
