using Azure.Messaging.ServiceBus;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Domain.Messages;

namespace DocumentProcessor.Infrastructure.Messaging
{
    public class ServiceBusMessagePublisher : IServiceBusMessagePublisher
    {
        private readonly ServiceBusSender _keyScanQueueSender;
        private readonly ServiceBusSender _patternExtractQueueSender;

        public ServiceBusMessagePublisher(ServiceBusClient client)
        {
            _keyScanQueueSender = client.CreateSender(Constants.Constants.CONST_KEYSCAN_QUEUE_NAME);
            _patternExtractQueueSender = client.CreateSender(Constants.Constants.CONST_PATTERN_EXTRACT_QUEUE_NAME);
        }

        public async Task PublishDocumentCreatedAsync(Guid documentId, string content, CancellationToken cancellationToken = default)
        {
            var message = new DocumentCreatedMessage(documentId, content, DateTime.UtcNow);

            var sbMessage = new ServiceBusMessage(BinaryData.FromObjectAsJson(message))
            {
                ContentType = "application/json",
                Subject = $"Document-{documentId}-Created",
                MessageId = documentId.ToString()
            };

            await _keyScanQueueSender.SendMessageAsync(sbMessage, cancellationToken);
        }

        public async Task PublishScanCompletedAsync(Guid documentId, string content, CancellationToken cancellationToken = default)
        {
            var message = new ScanCompletedMessage(documentId, content);

            var sbMessage = new ServiceBusMessage(BinaryData.FromObjectAsJson(message))
            {
                ContentType = "application/json",
                Subject = $"Scan-{documentId}-Completed",
                MessageId = documentId.ToString()
            };

            await _patternExtractQueueSender.SendMessageAsync(sbMessage, cancellationToken);
        }
    }
}
