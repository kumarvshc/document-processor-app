namespace DocumentProcessor.Console.Models
{
    public class AddDocumentRequest
    {
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Dictionary<string, string> Metadata { get; set; } = new();
    }
}
