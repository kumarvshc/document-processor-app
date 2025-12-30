using Azure;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;
using DocumentProcessor.Api.Azure;
using DocumentProcessor.Api.Mapping;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Application.Services;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using DocumentProcessor.Infrastructure.Messaging;
using DocumentProcessor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Api
{
    public static class DependencyInjectionRegister
    {
        public static async Task<IServiceCollection> RegisterDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var azureConfig = new AzureServiceConfiguration();

            builder.Configuration.GetSection("AzureServiceConfiguration").Bind(azureConfig);

            var keyVaultUrl = azureConfig.KeyVaultUrl ?? throw new InvalidOperationException("AzureServiceConfiguration:KeyVaultUrl missing");

            var managedIdentityClientId = azureConfig.UserManagedIdentityClientId ?? throw new InvalidOperationException("AzureServiceConfiguration:UserManagedIdentityClientId missing");

            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = managedIdentityClientId
            });

            var secretClient = new SecretClient(new Uri(keyVaultUrl), credential);

            var sqlConnectionString = await GetSecret(secretClient, "SqlConnection");

            if (sqlConnectionString is null)
                throw new InvalidOperationException("Sql ConnectionString missing");

            // Add DbContext
            services.AddDbContext<DocumentProcessorDbContext>(options =>
                options.UseSqlServer(sqlConnectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);

                }));

            // Register Mapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Add Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IScanResultRepository, ScanResultRepository>();

            // Add Application Services
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IServiceBusMessageService, ServiceBusMessageService>();

            // Service Bus
            var serviceBusConnection = await GetSecret(secretClient, "ServiceBusConnection");
            if (serviceBusConnection is null)
                throw new InvalidOperationException("Service Bus Connection missing");

            services.AddSingleton(new ServiceBusClient(serviceBusConnection));
            services.AddSingleton<IServiceBusMessagePublisher, ServiceBusMessagePublisher>();

            return services;
        }

        static async Task<string> GetSecret(SecretClient secretClient, string secretName)
        {
            try
            {
                var secret = await secretClient.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                throw new KeyNotFoundException($"The secret '{secretName}' was not found in Key Vault", ex);
            }
        }
    }
}
