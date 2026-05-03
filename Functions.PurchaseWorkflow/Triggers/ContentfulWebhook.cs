using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

public class ContentfulWebhook
{
    private readonly ILogger _logger;
    public ContentfulWebhook(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ContentfulWebhook>();
    }

    [Function("ContentfulWebhook")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "webhooks/contentful")]
        HttpRequestData req)
    {
        // Читаем тело
        var json = await req.ReadAsStringAsync();

        // Логируем (проверим в Azure Portal)
        _logger.LogInformation("Received webhook from Contentful:");
        _logger.LogInformation(json);

        // Возвращаем 200 OK
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("OK");
        return response;
    }
}