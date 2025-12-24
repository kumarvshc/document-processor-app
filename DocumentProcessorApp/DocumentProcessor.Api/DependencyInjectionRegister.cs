using DocumentProcessor.Application.ServiceInterfaces;
using DocumentProcessor.Application.Services;
using DocumentProcessor.Domain.Interfaces;
using DocumentProcessor.Infrastructure.Data;
using DocumentProcessor.Infrastructure.Repositories;

namespace DocumentProcessor.Api
{
    public static class DependencyInjectionRegister
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(_ => { }, typeof(Program));

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
