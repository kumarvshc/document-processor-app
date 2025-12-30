namespace DocumentProcessor.Application.DTO.Request;

public record AddDocumentRequest(
    string FileName,
    string Content,
    Dictionary<string, string>? Metadata = null
);