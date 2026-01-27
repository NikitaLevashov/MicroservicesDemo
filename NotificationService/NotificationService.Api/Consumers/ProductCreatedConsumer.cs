
using MassTransit;
using MediatR;
using NotificationService.Application.Notifications.Commands.CreateNotification;
using SharedMessaging.Contracts;


public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly ILogger<ProductCreatedConsumer> _logger;
    private readonly IMediator _mediator;

    public ProductCreatedConsumer(ILogger<ProductCreatedConsumer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<ProductCreated> context)
    {
        var evt = context.Message;

        _logger.LogInformation("Consuming ProductCreated: {ProductId}", evt.ProductId);

        var type = "ProductCreated";
        var recipient = "team@example.com";
        var message = $"Product created #{evt.ProductId}: {evt.Name} ({evt.Price}, qty={evt.StockQuantity})";

        return _mediator.Send(new CreateNotificationCommand(type, recipient, message), context.CancellationToken);
    }
}

