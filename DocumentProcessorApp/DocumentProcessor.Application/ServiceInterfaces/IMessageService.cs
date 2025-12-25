using DocumentProcessor.Domain.Entities;

namespace DocumentProcessor.Application.ServiceInterfaces
{
    public interface IMessageService
    {
        Task PublishDocumentCreatedAsync(Document document, CancellationToken cancellationToken = default);
    }
}
