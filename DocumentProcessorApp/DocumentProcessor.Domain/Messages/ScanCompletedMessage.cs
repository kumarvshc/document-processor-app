namespace DocumentProcessor.Domain.Messages;

public record ScanCompletedMessage(
    Guid DocumentId,
    string Content,
    bool IsDangerous,
    List<ScanResultDto> DangerousMatches
);

public record ScanResultDto(int Position, string MatchedText);
