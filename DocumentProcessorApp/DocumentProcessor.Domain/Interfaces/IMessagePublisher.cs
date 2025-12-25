namespace DocumentProcessor.Domain.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishDocumentCreatedAsync(Guid documentId, string fileName, string content, CancellationToken cancellationToken = default);
        Task PublishScanCompletedAsync(Guid documentId, string content, bool isDangerous, List<(int Position, string MatchedText)> dangerousMatches, CancellationToken cancellationToken = default);
    }
}
