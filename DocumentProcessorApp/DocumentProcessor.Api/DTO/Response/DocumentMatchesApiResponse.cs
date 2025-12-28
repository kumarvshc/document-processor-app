using DocumentProcessor.Application.DTO.Response;

namespace DocumentProcessor.Api.DTO.Response;

public record DocumentMatchesApiResponse(
 Guid DocumentId,
    string FileName,
    DateTime ProcessedDateTime,
    IEnumerable<ScanResultResponse> Matches);