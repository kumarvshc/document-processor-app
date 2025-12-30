namespace DocumentProcessor.Api.DTO.Request
{
    public class AddDocumentApiRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;      
        public Dictionary<string, string>? Metadata { get; set; }
    }
}
