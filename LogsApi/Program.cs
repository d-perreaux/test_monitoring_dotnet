using System;
using Serilog;
using Serilog.Debugging;
using System.Threading;
using Serilog.Sinks.Http.BatchFormatters;

// var builder = WebApplication.CreateBuilder(args);
// Log.Logger = new LoggerConfiguration()
//     .MinimumLevel.Debug()
//     .WriteTo.Console(
//         outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"    )
//     .WriteTo.File(
//         "logs/LogsApi.txt",
//         rollingInterval: RollingInterval.Day, 
//          outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] (v{Version}) {Message:lj}{NewLine}{Exception}"
//         )
//     .Enrich.WithProperty("Version", "0.3")
//     // .WriteTo.Http("http://logstash:5044");
//     .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
        .Build();

    SelfLog.Enable(Console.Error);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    builder.Services.AddSerilog();
    
    var app = builder.Build();


    app.MapGet("/", () =>
{
    Log.Information("Hello !! ");
    int a = 10, b = 0;
    try
    {
        Log.Debug("Dividing {A} by {B}", a, b);
        Console.WriteLine(a / b);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Division issue");
    }
    return "Hello World!";
});

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
