
using Functions.PurchaseWorkflow.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.Configure<TaskServiceOptions>(options =>
{
    options.BaseUrl = builder.Configuration["TaskService:BaseUrl"]
        ?? builder.Configuration["Values:TaskService:BaseUrl"]
        ?? "http://localhost:5088";
});


builder.Services.AddHttpClient<TaskServiceClient>((sp, http) =>
{
    var opts = sp.GetRequiredService<IOptions<TaskServiceOptions>>().Value;
    http.BaseAddress = new Uri(opts.BaseUrl.TrimEnd('/') + "/"); // важно — конечный слэш
    http.Timeout = TimeSpan.FromSeconds(30);                     // общий таймаут
    http.DefaultRequestHeaders.ExpectContinue = false;
})
.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    ConnectTimeout = TimeSpan.FromSeconds(5),
    MaxConnectionsPerServer = 256,
    PooledConnectionLifetime = TimeSpan.FromMinutes(2),
    PooledConnectionIdleTimeout = TimeSpan.FromSeconds(30),
    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
});


builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var app = builder.Build();

// Startup diagnostic
var startupLogger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
var opts = app.Services.GetRequiredService<IOptions<TaskServiceOptions>>().Value;
startupLogger.LogInformation("TaskService BaseUrl: {BaseUrl}", opts.BaseUrl);

app.Run();
