namespace DocumentProcessor.Domain.Messages;

public record DocumentCreatedMessage(
    Guid DocumentId,
    string Content
);