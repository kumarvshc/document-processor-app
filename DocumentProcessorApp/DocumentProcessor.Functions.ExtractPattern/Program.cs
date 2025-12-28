using Azure.Messaging.ServiceBus;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using DocumentProcessor.Infrastructure.Messaging;
using DocumentProcessor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Add DbContext
        var connectionString = Environment.GetEnvironmentVariable("SqlConnection");
        services.AddDbContext<DocumentProcessorDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Add Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    })
    .Build();
host.Run();