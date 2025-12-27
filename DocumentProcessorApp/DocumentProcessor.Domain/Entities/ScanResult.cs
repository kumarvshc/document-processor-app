using DocumentProcessor.Domain.Enums;

namespace DocumentProcessor.Domain.Entities
{
    public class ScanResult
    {
        public Guid Id { get; private set; }
        public Guid DocumentId { get; private set; }
        public ScanType ScanType { get; private set; }
        public int Position { get;private set; }
        public string MatchedText { get; private set; } = string.Empty;
        public DateTime CreatedDateTime { get; private set; }
        public Document Document { get; private set; } = null;

        private ScanResult()
        {

        }

        public static ScanResult Create(Guid documentId, ScanType scanType, int position, string matchedText)
        {
            if (position < 0)
                throw new ArgumentException("Position cannot be negative.", nameof(position));

            if (string.IsNullOrWhiteSpace(matchedText))
                throw new ArgumentException("Matched text cannot be empty.", nameof(matchedText));

            return new ScanResult
            {
                Id = Guid.NewGuid(),
                DocumentId = documentId,
                ScanType = scanType,
                Position = position,
                MatchedText = matchedText,
                CreatedDateTime = DateTime.UtcNow
            };
        }
    }
}
