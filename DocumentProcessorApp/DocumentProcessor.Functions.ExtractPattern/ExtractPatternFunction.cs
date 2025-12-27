using Azure.Messaging.ServiceBus;
using DocumentProcessor.Domain.Entities;
using DocumentProcessor.Domain.Enums;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Domain.Messages;
using Microsoft.Azure.Functions.Worker;
using System.Text.RegularExpressions;

namespace DocumentProcessor.Functions.ExtractPattern
{
    public class ExtractPatternFunction
    {
        private readonly IUnitOfWork _unitOfWork;

        private static readonly Regex PatternRegex = new Regex(Constants.Constants.CONST_REGEX_PATTERN, RegexOptions.Compiled);

        public ExtractPatternFunction(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Function(nameof(ExtractPatterns))]
        public async Task ExtractPatterns([ServiceBusTrigger(Constants.Constants.CONST_PATTERN_EXTRACT_QUEUE_NAME, Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message, CancellationToken cancellationToken)
        {
            var scanMessage = message.Body.ToObjectFromJson<ScanCompletedMessage>();

            var matches = PatternRegex.Matches(scanMessage.Content);

            var matchCount = 0;

            foreach (Match match in matches)
            {
                var scanResult = ScanResult.Create(
                    scanMessage.DocumentId,
                    ScanType.PatternMatch,
                    match.Index,
                    match.Value);

                await _unitOfWork.ScanResults.AddAsync(scanResult, cancellationToken);

                matchCount++;
            }

            var document = await _unitOfWork.Documents.GetByIdAsync(scanMessage.DocumentId, cancellationToken);

            if (document is not null)
            {
                document.MarkAsAvailable();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
