using System.Collections.ObjectModel;

namespace DocumentProcessor.Application.DTO.Response;

public record DocumentTextResponse(
    Guid Id,
    string FileName,
    string Content
);