using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Application.DTO.Response;

    public record DocumentResponse(
    Guid Id,
    string FileName,
    DocumentStatus Status,
    DateTime CreatedDateTime
);

