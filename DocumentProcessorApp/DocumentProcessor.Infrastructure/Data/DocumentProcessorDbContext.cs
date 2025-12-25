using DocumentProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Document = DocumentProcessor.Domain.Entities.Document;

namespace DocumentProcessor.Infrastructure.Data
{
    public class DocumentProcessorDbContext : DbContext
    {
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<ScanResult> ScanResults => Set<ScanResult>();

        public DocumentProcessorDbContext(DbContextOptions<DocumentProcessorDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentProcessorDbContext).Assembly);

            var dictToJsonConverter = new ValueConverter<Dictionary<string, string>, string>(v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>());

            modelBuilder.Entity<Document>()
                .Property(d => d.Metadata)
                .HasConversion(dictToJsonConverter)
                .HasColumnType("nvarchar(max)");

        }
    }
}
