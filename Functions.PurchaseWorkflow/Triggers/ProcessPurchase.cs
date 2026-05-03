using System.Text.Json;
using Functions.PurchaseWorkflow.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PurchaseService.Infrastructure.Queues;

namespace Functions.PurchaseWorkflow.Triggers;

public class ProcessPurchase
{
    private readonly ILogger<ProcessPurchase> _logger;
    private readonly TaskServiceClient _taskClient;
    private readonly QueueOptions _opts;

    public ProcessPurchase(
        ILogger<ProcessPurchase> logger,
        TaskServiceClient taskClient,
        IOptions<QueueOptions> opts)
    {
        _logger = logger;
        _taskClient = taskClient;
    }

    [Function("ProcessPurchase")]
    public async Task Run(
        [QueueTrigger("purchases", Connection = "AzureWebJobsStorage")] string raw,
        FunctionContext context)
    {
        var ct = context.CancellationToken;

        // 0) Diagnostics — see exactly what came from the queue
        _logger.LogInformation("RAW queue message: {raw}", raw);

        // 1) Deserialize (case-insensitive to be robust to camelCase/PascalCase)
        PurchaseCreatedMessage? msg;
        try
        {
            msg = JsonSerializer.Deserialize<PurchaseCreatedMessage>(
                raw,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException jex)
        {
            _logger.LogWarning(jex, "Invalid JSON in queue message. Skipping without throwing.");
            return; // don't throw → avoid sending to poison
        }

        if (msg is null)
        {
            _logger.LogWarning("Deserialization returned null. Skipping without throwing.");
            return; // don't throw → avoid sending to poison
        }

        _logger.LogInformation(
            "Processing purchase: PurchaseId={PurchaseId}, ClientId={ClientId}, Total={Total}",
            msg.PurchaseId, msg.ClientId, msg.Total);

        // 2) Build DTO for TaskService
        var dto = new CreateTaskDto(
            Status: "New",
            Title: $"Handle purchase #{msg.PurchaseId}",
            Description: $"Check payment / stock for client #{msg.ClientId}; total={msg.Total}",
            PurchaseId: msg.PurchaseId
        );

        // 3) Call TaskService with clear diagnostics
        try
        {
           var response = await _taskClient.CreateTaskAsync(dto, ct);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError(
                    "TaskService returned non-success status. StatusCode={StatusCode}. Body={Body}",
                    (int)response.StatusCode, body);

                // Throw to trigger built-in queue retry; after max retries → poison queue.
                response.EnsureSuccessStatusCode();
            }

            _logger.LogInformation(
                "Task for purchase #{PurchaseId} was successfully created in TaskService.",
                msg.PurchaseId);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx,
                "HTTP error when calling TaskService. Check BaseUrl/port/reachability. Will retry via queue retry policy.");
            throw; // let the queue retry
        }
        catch (TaskCanceledException tcex) when (!ct.IsCancellationRequested)
        {
            _logger.LogError(tcex,
                "Timeout when calling TaskService. Will retry via queue retry policy.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error during message processing. Will retry via queue retry policy.");
            throw;
        }
    }
}