
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Abstraction;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Notifications.Queries;
using NotificationService.Applications.Abstractions;
using NotificationService.Infrastructure.DomainEvents;
using NotificationService.Infrastructure.Persistence;
using NotificationService.Infrastructure.Persistence.MongoDB;
using NotificationService.Infrastructure.Persistence.Read;
using NotificationService.Infrastructure.Persistence.Repositories;
using NotificationService.Infrastructure.Persistence.Write;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;

// MediatR
builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssembly(typeof(GetAllNotificationsQuery).Assembly));

// EF Core
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(cfg.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<NotificationMongoRepository>();


// Repositories + UoW + DomainEventsDispatcher
builder.Services.AddScoped<INotificationReadRepository, EfNotificationReadRepository>();
builder.Services.AddScoped<INotificationWriteRepository, EfNotificationWriteRepository>();
builder.Services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// MassTransit + EF Outbox + RabbitMQ
builder.Services.AddMassTransit(x =>
{
    // EF Outbox для атомарной публикации интеграционных событий (если будешь публиковать)
    x.AddEntityFrameworkOutbox<NotificationDbContext>(o =>
    {
        o.UseSqlServer();
        o.QueryDelay = TimeSpan.FromSeconds(1);
        o.DuplicateDetectionWindow = TimeSpan.FromMinutes(1);
        o.UseBusOutbox();
    });



    x.AddConsumer<ProductCreatedConsumer>();

    x.UsingRabbitMq((context, busCfg) =>
    {
        busCfg.Host(cfg["RabbitMq:Host"] ?? "localhost", h =>
        {
            h.Username(cfg["RabbitMq:User"] ?? "guest");
            h.Password(cfg["RabbitMq:Password"] ?? "guest");
        });

        busCfg.ReceiveEndpoint("notification-product-created", e =>
        {
            e.ConfigureConsumer<ProductCreatedConsumer>(context);
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));
        });

        busCfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("notification-service", false));
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
