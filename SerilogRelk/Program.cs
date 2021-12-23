using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using System;
using System.IO;

namespace SerilogRelk
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration =  new ConfigurationBuilder().SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration).Enrich.FromLogContext();

            Logger logger = loggerConfiguration.CreateLogger();

            logger.Information("Information sample");
            logger.Warning("Warning sample");

            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddSerilog(logger, true));

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            ILogger<Program> msLogger = serviceProvider.GetRequiredService<ILogger<Program>>();

            msLogger.LogError(new EventId(500, "UnhandledException"), new ArgumentNullException("msg"),
                "Some error {P1} {p2} {@p3} {p4} {p5}", 123, DateTime.UtcNow, new { a = "qwe", b = 112 }, new decimal(34.7), 34.7f);
        }
    }
}
