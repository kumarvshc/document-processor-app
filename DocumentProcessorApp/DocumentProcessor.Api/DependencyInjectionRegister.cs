using DocumentProcessor.Api.Mapping;
using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Application.Services;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using DocumentProcessor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Api
{
    public static class DependencyInjectionRegister
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            // Add DbContext   
            var connectionString = builder.Configuration.GetConnectionString("SqlConnection")
                      ?? throw new InvalidOperationException("Sql ConnectionString missing");
            services.AddDbContext<DocumentProcessorDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
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
           
            return services;
        }
    }
}
