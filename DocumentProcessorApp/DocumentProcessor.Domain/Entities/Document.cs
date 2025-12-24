using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public DocumentStatus Status { get; private set; }
        public DateTime CreatedDateTime { get; private set; }
        public DateTime ProcessedDateTime { get; private set; }

        private Document()
        {

        }

        public static Document Create(string fileName, string content, int maxContentSize = 1024)
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
                Status = DocumentStatus.Processing,
                CreatedDateTime = DateTime.UtcNow
            };
        }
    }
}
