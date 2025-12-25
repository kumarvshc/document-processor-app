namespace DocumentProcessor.Application.DTO.Request;

public record AddDocumentRequest(
    string FileName,
    string Content,
    int MaxContextSize,
    Dictionary<string, string>? Metadata = null
);