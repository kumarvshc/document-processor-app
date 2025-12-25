using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Application.DTO.Response;

public record DocumentMatchesResponse(
    Guid DocumentId,
    string FileName,
    IEnumerable<ScanResultResponse> Matches
);
