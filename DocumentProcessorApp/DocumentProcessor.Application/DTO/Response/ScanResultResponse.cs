using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Application.DTO.Response;
public record ScanResultResponse(
    Guid Id,
    int Position,
    ScanType ScanType,
    string MatchedText,
    DateTime CreatedDateTime
);
