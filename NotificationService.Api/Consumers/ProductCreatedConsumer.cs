
using MassTransit;
using SharedMessaging.Contracts;

public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly ProductCreatedHandler _handler;

    public ProductCreatedConsumer(ProductCreatedHandler handler)
    {
        _handler = handler;
    }

    public Task Consume(ConsumeContext<ProductCreated> context)
        => _handler.Handle(context.Message, context.CancellationToken);
}
