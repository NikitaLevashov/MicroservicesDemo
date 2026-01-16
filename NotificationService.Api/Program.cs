//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();




using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Services;
using NotificationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// … регистрируешь DI NotificationServiceApp, DbContext и т.д.
builder.Services.AddScoped<ProductCreatedHandler>(); // Application handler

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", h => { h.Username("guest"); h.Password("guest"); });
        cfg.ReceiveEndpoint("notification-product-created", e =>
        {
            e.ConfigureConsumer<ProductCreatedConsumer>(ctx);
            e.PrefetchCount = 16;
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));
        });
        cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("notification-service", false));
    });
});
//builder.Services.AddMassTransitHostedService(true);


// Add services
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<INotificationService, NotificationServiceApp>();

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

