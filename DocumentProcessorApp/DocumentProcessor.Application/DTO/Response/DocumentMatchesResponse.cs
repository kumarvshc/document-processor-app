namespace DocumentProcessor.Application.DTO.Response;

public record DocumentMatchesResponse(
    Guid DocumentId,
    string FileName,
    DateTime ProcessedDateTime,
    IEnumerable<ScanResultResponse> Matches
);
