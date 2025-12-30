using DocumentProcessor.Application.DTO.Request;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Application.Services;
using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Interfaces;
using Moq;

namespace DocumentProcessor.Tests
{
    public class DocumentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IServiceBusMessageService> _serviceBusMessageServiceMock;
        private readonly DocumentService _documentService;
        public DocumentServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _serviceBusMessageServiceMock = new Mock<IServiceBusMessageService>();
            _documentService = new DocumentService(_unitOfWorkMock.Object, _serviceBusMessageServiceMock.Object);
        }

        [Fact]
        public async Task AddDocumentAsync_EmptyFileName_ThrowsArgumentException()
        {
            var request = new AddDocumentRequest("", "content", 1024, null);

            await Assert.ThrowsAsync<ArgumentException>(() => _documentService.AddDocumentAsync(request));
        }


        [Fact]
        public async Task AddDocumentAsync_ValidRequest_ReturnsSuccess()
        {
            var request = new AddDocumentRequest("test.txt", "content", 1024, null);

            _unitOfWorkMock.Setup(u => u.Documents.AddAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _serviceBusMessageServiceMock.Setup(m => m.PublishDocumentCreatedAsync(It.IsAny<Document>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var result = await _documentService.AddDocumentAsync(request);
            
            Assert.True(result.IsSuccess);

            Assert.NotNull(result.Value);

            Assert.Equal("test.txt", result.Value.FileName);
        }
    }
}
