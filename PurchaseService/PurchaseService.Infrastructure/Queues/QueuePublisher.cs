using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace PurchaseService.Infrastructure.Queues
{

    public class QueuePublisher
    {
        private readonly QueueClient _queue;
        private readonly QueueOptions _opts;
        private readonly ILogger<QueueOptions> _logger;

        public QueuePublisher(IOptions<QueueOptions> opts, ILogger<QueueOptions> logger)
        {
            var options = new QueueClientOptions
            {
                // Явно просим Base64 у SDK (обычно это и так дефолт, но лучше указать):
                MessageEncoding = QueueMessageEncoding.Base64
            };

            _queue = new QueueClient("UseDevelopmentStorage=true", "purchases", options);
            _queue.CreateIfNotExists();
            _opts = opts.Value;
            _logger = logger;
        }

        public Task PublishAsync(object message, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(message);


            _logger.LogInformation("Publishing to queue '{Queue}' (initialVisibility={Initial}, ttl={Ttl})",
                        _opts.PurchasesQueueName, _opts.InitialVisibility, _opts.TimeToLive);

            //return _queue.SendMessageAsync(json, cancellationToken: ct);


            return _queue.SendMessageAsync(
                        json,
                        visibilityTimeout: _opts.InitialVisibility,
                        timeToLive: _opts.TimeToLive,
                        cancellationToken: ct);

        }
    }

}
