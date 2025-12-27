using DocumentProcessor.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessor.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        [NotMapped]
        public Dictionary<string, string> Metadata { get; private set; } = new();
        public DocumentStatus Status { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime ProcessedDateTime { get; private set; }

        private readonly List<ScanResult> _scanResults = [];
        public IReadOnlyCollection<ScanResult> ScanResults => _scanResults.AsReadOnly();

        private Document()
        {

        }

        public static Document Create(string fileName, string content, Dictionary<string,string> metadata, int maxContentSize = 1024)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be empty.", nameof(fileName));

            if (content.Length > maxContentSize)
                throw new ArgumentException($"Document content cannot be exceed {maxContentSize}KB", nameof(content));

            return new Document
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                Content = content,
                Metadata = new Dictionary<string, string>(metadata ?? new()),
                Status = DocumentStatus.Processing,
                CreatedDateTime = DateTime.UtcNow
            };
        }

        public void AddScanResult(ScanResult scanResult)
        {
            _scanResults.Add(scanResult);
        }

        public void MarkAsAvailable()
        {
            Status = DocumentStatus.Available;
            ProcessedDateTime = DateTime.UtcNow;
        }
    }
}
