using DocumentProcessor.Application.DTO.Response;
using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Api.DTO.Response;

public record DocumentMatchesApiResponse(
 Guid DocumentId,
    string FileName,
    IEnumerable<ScanResultResponse> Matches);