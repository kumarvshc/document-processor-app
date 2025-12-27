namespace DocumentProcessor.Domain.Messages;

public record ScanCompletedMessage(
    Guid DocumentId,
    string Content
);
