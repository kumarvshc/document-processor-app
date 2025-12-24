using DocumentProcessor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
        }
    }
}
