namespace DocumentProcessor.Domain.Interfaces
{
    public interface IServiceBusMessagePublisher
    {
        Task PublishDocumentCreatedAsync(Guid documentId, string content, CancellationToken cancellationToken = default);
        Task PublishScanCompletedAsync(Guid documentId, string content, CancellationToken cancellationToken = default);
    }
}
