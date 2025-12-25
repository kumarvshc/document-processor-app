using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Application.DTO.Response;

public record DocumentStatusResponse(
    Guid Id,
    DocumentStatus Status
);