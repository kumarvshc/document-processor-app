using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DocumentProcessor.Api.HealthCheck
{
    public class HealtchCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Document processor application is healthy."));
        }
    }
}
