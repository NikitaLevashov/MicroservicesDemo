var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



//using Microsoft.EntityFrameworkCore;
//using TaskService.Infrastructure;
//using TaskService.Application.Interfaces;
//using TaskService.Application.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services
//builder.Services.AddDbContext<TaskDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskDb")));

//builder.Services.AddScoped<ITaskService, TaskServiceApp>();

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.MapControllers();
//app.Run();

