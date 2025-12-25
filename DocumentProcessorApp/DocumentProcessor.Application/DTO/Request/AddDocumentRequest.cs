namespace DocumentProcessor.Application.DTO.Request;

public record AddDocumentRequest(
    string FileName,
    string Content,
    int MaxContentSize,
    Dictionary<string, string>? Metadata = null
);