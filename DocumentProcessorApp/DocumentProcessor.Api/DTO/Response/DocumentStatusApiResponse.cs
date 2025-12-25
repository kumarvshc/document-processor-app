using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Api.DTO.Response;

public record DocumentStatusApiResponse(
    Guid Id,
    DocumentStatus Status
);