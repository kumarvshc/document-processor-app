using Azure.Messaging.ServiceBus;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Enums;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Domain.Messages;
using Microsoft.Azure.Functions.Worker;

namespace DocumentProcessor.Functions.KeyScanner
{
    public class KeyScannerFunction
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageService _messageService;

        public KeyScannerFunction(IUnitOfWork unitOfWork, IMessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
        }

        [Function("KeyScanDocument")]
        public async Task ScanForDangerousContent([ServiceBusTrigger(Constants.Constants.CONST_KEYSCAN_QUEUE_NAME, Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message, CancellationToken cancellationToken = default)
        {
            var documentMessage = message.Body.ToObjectFromJson<DocumentCreatedMessage>();

            var content = documentMessage.Content;

            var contentLower = content.ToLowerInvariant();

            const string searchWord = Constants.Constants.CONST_KEYWORD_SCAN;

            var index = 0;

            bool isDangerousFound = false;

            while ((index = contentLower.IndexOf(searchWord, index, StringComparison.Ordinal)) != -1)
            {
                var scanResult = ScanResult.Create(
                    documentMessage.DocumentId,
                    ScanType.DangerousKeyWord,
                    index,
                    searchWord);

                await _unitOfWork.ScanResults.AddAsync(scanResult, cancellationToken);

                isDangerousFound = true;

                index += searchWord.Length;
            }

            if (isDangerousFound)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _messageService.PublishScanCompletedAsync(documentMessage.DocumentId, documentMessage.Content, cancellationToken);
        }
    }
}
