using Azure.Messaging.ServiceBus;
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
        private readonly IMessagePublisher _messagePublisher;

        public KeyScannerFunction(IUnitOfWork unitOfWork, IMessagePublisher messagePublisher)
        {
            _unitOfWork = unitOfWork;
            _messagePublisher = messagePublisher;
        }

        [Function("KeyScanDocument")]
        public async Task ScanForDangerousContent([ServiceBusTrigger(Constants.Constants.CONST_KEYSCAN_QUEUE_NAME, Connection = "ServiceBusConnection")] ServiceBusReceivedMessage messagee, CancellationToken cancellationToken = default)
        {
            var documentMessage = messagee.Body.ToObjectFromJson<DocumentCreatedMessage>();

            var dangerousMatches = new List<(int Position, string MatchedText)>();

            var content = documentMessage.Content;

            var contentLower = content.ToLowerInvariant();

            const string searchWord = "dangerous";

            var index = 0;

            while ((index = contentLower.IndexOf(searchWord, index, StringComparison.Ordinal)) != -1)
            {
                var scanResult = ScanResult.Create(
                    documentMessage.DocumentId,
                    ScanType.DangerousKeyWord,
                    index,
                    searchWord);

                await _unitOfWork.ScanResults.AddAsync(scanResult, cancellationToken);
                dangerousMatches.Add((index, searchWord));
                index += searchWord.Length;
            }

            if (dangerousMatches.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            await _messagePublisher.PublishScanCompletedAsync(
            documentMessage.DocumentId,
            documentMessage.Content,
            dangerousMatches.Count > 0,
            dangerousMatches,
            cancellationToken);

        }
    }
}
