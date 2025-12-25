// Configure Serilog
using DocumentProcessor.Api;
using DocumentProcessor.Api.HealthCheck;
using DocumentProcessor.Api.Middleware;
using DocumentProcessor.Infrastructure.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "DocumentProcessorApi")
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/document-processor-.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add FluentValidation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Document Processor API",
            Version = "v1",
            Description = "Dcument Processor API "
        });
    });

    // Add Health Checks
    builder.Services.AddHealthChecks().AddCheck<HealtchCheck>("doc-processor-health-check");

    builder.Services.RegisterDependencies(builder);

    // Display the enum text instead of number
    builder.Services.AddControllers().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Configure exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHsts();

    app.UseHttpsRedirection();

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    // Map Health Checks
    app.MapHealthChecks("/health");

    app.MapControllers();

    app.Run();

}
finally
{
    Log.CloseAndFlush();
}