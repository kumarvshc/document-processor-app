using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Infrastructure.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository( DocumentProcessorDbContext context) : base(context)
        {                
        }
    }
}
