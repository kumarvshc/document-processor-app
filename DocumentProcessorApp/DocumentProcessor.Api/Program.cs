// Configure Serilog
using DocumentProcessor.Api;
using DocumentProcessor.Api.HealthCheck;
using DocumentProcessor.Api.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
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

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Document Processor API",
            Version = "v1",
            Description = "Document Processor API"
        });
    });

    // Add Health Checks
    builder.Services.AddHealthChecks().AddCheck<HealthCheck>("doc-processor-health-check");

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
    
    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    // Configure exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseHsts();

    app.UseHttpsRedirection();   

    app.UseAuthorization();

    // Map Health Checks
    app.MapHealthChecks("/health");

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Error(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}