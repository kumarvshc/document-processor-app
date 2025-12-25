namespace DocumentProcessor.Domain.Messages;

public record DocumentCreatedMessage(
    Guid DocumentId,
    string FileName,
    string Content,
    DateTime CreatedDateTime
);