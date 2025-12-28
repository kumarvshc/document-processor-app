using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Api.DTO.Response;

public record AddDocumentApiResponse(
 Guid Id,
 string FileName,
 DocumentStatus Status,
 DateTime CreatedDateTime);