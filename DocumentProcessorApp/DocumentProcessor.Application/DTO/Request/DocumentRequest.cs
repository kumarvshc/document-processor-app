namespace DocumentProcessor.Application.DTO.Request
{
    public class DocumentRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int MaxContextSize { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }
}
