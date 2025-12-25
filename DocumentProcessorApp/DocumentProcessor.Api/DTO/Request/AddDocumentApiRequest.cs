using System.ComponentModel.DataAnnotations;

namespace DocumentProcessor.Api.DTO.Request
{
    public class AddDocumentApiRequest
    {
        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(1024)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int MaxContextSize { get; set; }

        public Dictionary<string, string>? Metadata { get; set; }
    }
}
