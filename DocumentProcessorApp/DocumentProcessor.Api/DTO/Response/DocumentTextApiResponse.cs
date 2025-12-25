namespace DocumentProcessor.Api.DTO.Response;
public record DocumentTextApiResponse(
    Guid Id,
    string FileName,
    string Content
);