////var builder = WebApplication.CreateBuilder(args);

////// Add services to the container.

////builder.Services.AddControllers();
////// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
////builder.Services.AddSwaggerGen();

////var app = builder.Build();

////// Configure the HTTP request pipeline.
////if (app.Environment.IsDevelopment())
////{
////    app.UseSwagger();
////    app.UseSwaggerUI();
////}

////app.UseHttpsRedirection();

////app.UseAuthorization();

////app.MapControllers();

////app.Run();



//using Microsoft.EntityFrameworkCore;
//using ProductService.Application.Interfaces;
//using ProductService.Application.Services;
//using ProductService.Infrastructure.Persistence;


//var builder = WebApplication.CreateBuilder(args);

//// Add services
//builder.Services.AddDbContext<ProductDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<IProductService, ProductServiceApp>();

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//        options.JsonSerializerOptions.WriteIndented = true; // ???????????
//    });


//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.MapControllers();

////app.Run((context) => {

////    context.Response.ContentType = "application/json";
////    return context.Response.WriteAsync("text test Mikita");

////    }
////);
//app.Run();




using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductService.Application.Interfaces;
using ProductService.Application.Services;
using ProductService.Infrastructure.Persistence;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ==============================
// 1) СЕРВИСЫ (Dependency Injection)
// ==============================

// База данных
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Бизнес-логика
builder.Services.AddTransient<IProductService, ProductServiceApp>();

// Контроллеры + JSON-форматирование для MVC (Controllers)
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Разрешение циклических ссылок через $id/$ref
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Настройки JSON по умолчанию для минимальных API/Results (если их используешь)
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.SerializerOptions.WriteIndented = true;
});

// Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "API для работы с товарами"
    });
});

// CORS (при необходимости; пример — разрешить локальную разработку UI)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .WithOrigins("https://localhost:3000", "http://localhost:3000") // фронтенд-URL в dev
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Health Checks (удобно для наблюдения)
builder.Services.AddHealthChecks();

// ==============================
// 2) ПРИЛОЖЕНИЕ И MIDDLEWARE
// ==============================

var app = builder.Build();

// Dev-инструменты
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
        options.RoutePrefix = "swagger"; // UI будет доступен по /swagger
    });

    app.UseCors("DevCors");
}
else
{
    // В проде Swagger можно отключить или оставить только JSON-описание
    // app.UseSwagger();
    // app.UseSwaggerUI(); // обычно выключают в проде
}

// Безопасность/инфраструктура
app.UseHttpsRedirection();

// Если планируешь авторизацию/аутентификацию:
app.UseAuthentication();
app.UseAuthorization();

//string date = "";
//app.MapGet("/", () => Results.Text("text test Mikita", "text/plain"));
//app.Use(async (context, next) =>
//{
//    date = DateTime.Now.ToShortDateString();
//    await next.Invoke();                 // вызываем middleware из app.Run
//    context.Response.WriteAsync($"  Test after Terminated Date: {date}");  // Current date: 08.12.2021
//});

////app.MapGet("/", () => Results.Text("text test Mikita", "text/plain"));

//app.Run(async (context) => await context.Response.WriteAsync($"Date: {date}"));

//app.Run();

// ==============================
// 3) ЭНДПОИНТЫ
// ==============================

// Корневой минимальный эндпоинт — не блокирует конвейер (вместо терминального app.Run(context => ...))
//app.MapGet("/", () => Results.Text("text test Mikita", "text/plain"));


//app.MapGet("/test", async (HttpContext context) =>
//{
//    var response = context.Response;

//    var dictionary = context.Request.Query;


//    response.ContentType = "text/plain; charset=utf-8";
//    response.Headers.Append("Content-Language", "ru-RU");
//    response.Headers.Append("secret-id", "256"); // кастомный заголовок

//    await response.WriteAsync("Привет METANIT.COM");
//});

//app.MapGet("redirect", (context) =>
//{
//    if (context.Request.Path == "/redirect")
//    {
//       context.Response.Redirect("https://www.google.com/search?q=metanit.com");

//        return Task.CompletedTask;

//    }

//    return Task.CompletedTask;
//});


//app.MapGet("/redirect", () =>
//{
//    Results.Redirect("https://www.google.com/search?q=metanit.com");
//});

//app.MapGet("/redirect", async (HttpContext context) =>
//{
//    if (context.Request.Path == "/redirect")
//    {
//        return Results.Redirect("https://www.google.com/search?q=metanit.com");
//    }

//    return Results.NotFound();
//});


//app.MapGet("/redirect", async (HttpContext context) =>
//{
//    if (context.Request.Path == "/redirect")
//    {
//        context.Response.Redirect("https://www.google.com/search?q=metanit.com");
//    }

//    // так как RequestDelegate должен вернуть Task
//    await Task.CompletedTask;
//});


// так как RequestDelegate должен вернуть Task








//app.(async (context) =>
//{
//    var response = context.Response;
//    response.Headers.ContentLanguage = "ru-RU";
//    response.Headers.ContentType = "text/plain; charset=utf-8";
//    response.Headers.Append("secret-id", "256");    // добавление кастомного заголовка
//    await response.WriteAsync("Привет METANIT.COM");
//});

// Health check


// Контроллеры
//app.MapControllers();

//app.MapGet("/", () => Results.Text("text test Mikita", "text/plain"));
//app.Use(async (ctx, next) =>
//{
//    var endpointBefore = ctx.GetEndpoint()?.DisplayName ?? "<none>";
//    Console.WriteLine($"[Before] {ctx.Request.Method} {ctx.Request.Path} endpoint={endpointBefore}");
//    await next();
//    var endpointAfter = ctx.GetEndpoint()?.DisplayName ?? "<none>";
//    Console.WriteLine($"[After ] {ctx.Request.Method} {ctx.Request.Path} endpoint={endpointAfter}");
//});


//app.Use(async (context, next) =>
//{
//    context.Items["date"] = DateTime.Now.ToShortDateString();
//    await next();
//});

////app.MapGet("/", () => Results.Text("text test Mikita", "text/plain"));

//app.MapGet("/date", (HttpContext ctx) =>
//{
//    var date = (string?)ctx.Items["date"] ?? "n/a";
//    return Results.Text($"Date: {date}", "text/plain");
//});

//// Fallback: сработает, только если ни один Map* не совпал
//app.Run(context =>
//{
//    var date = (string?)context.Items["date"] ?? "n/a";
//    return context.Response.WriteAsync($"Fallback Date: {date}");
//});


//// =======================app.MapHealthChecks("/health");=======
//// 4) ЗАПУСК
//// ==============================
//app.Run();

//app.Use(async (context, next) =>
//{
//    await next.Invoke();
//    await context.Response.WriteAsync("  TestDate1  ");
//});

//app.Use(async (context, next) =>
//{
//    string? path = context.Request.Path.Value?.ToLower();
//    if (path == "/date")
//    {
//        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
//    }
//    else
//    {
//        await next.Invoke();
//        await context.Response.WriteAsync(" After Invoke in Else ")
//    }
//});


//app.Run();
//async Task GetDate(HttpContext context, Func<Task> next)
//{
//    string? path = context.Request.Path.Value?.ToLower();
//    if (path == "/date")
//    {
//        await context.Response.WriteAsync($"Date: {DateTime.Now.ToShortDateString()}");
//    }
//    else
//    {
//        await next.Invoke();
//    }
//}

//app.UseWhen(
//    context => context.Request.Path == "/time", // условие: если путь запроса "/time"
//    appBuilder =>
//    {

//        appBuilder.Map("/time", timeApp =>
//        {
//            timeApp.Use(async (ctx, next) =>
//            {
//                Console.WriteLine($"Time: {DateTime.Now.ToShortTimeString()}");
//                await next();
//            });

//            timeApp.Run(async ctx =>
//            {
//                await ctx.Response.WriteAsync($"Time: {DateTime.Now.ToShortTimeString()}");
//            });
//        });


//        appBuilder.Use(async (context, next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            context.Response.WriteAsync(time);
//            await next();   // вызываем следующий middleware
//            context.Response.WriteAsync("  TestTime==  ");
//        });

//        appBuilder.Use(async (HttpContext context, Func<Task> next) =>
//        {
//            var time = DateTime.Now.ToShortTimeString();
//            await next();
//            context.Response.WriteAsync("Test 2 2 2 2 2");
//        });
//    });

//app.Run(async context =>
//{
//    await context.Response.WriteAsync("  Hello METANIT.COM  ");
//});


app.UseRouting();

//// Диагностика endpoint — только лог, НЕ пишем в тело
//app.Use(async (ctx, next) =>
//{
//    var before = ctx.GetEndpoint();
//    System.Diagnostics.Debug.WriteLine(before is null
//        ? "[ROUTING] BEFORE: no endpoint selected"
//        : $"[ROUTING] BEFORE: {before.DisplayName}");

//    await next();

//    var after = ctx.GetEndpoint();
//    System.Diagnostics.Debug.WriteLine(after is null
//        ? "[ROUTING] AFTER: still no endpoint (map-branch?)"
//        : $"[ROUTING] AFTER: {after.DisplayName}");
//});

// Контроллеры по атрибутам
app.MapControllers();

//// Диагностический endpoint — список endpoints
//app.MapGet("/routes", (IEnumerable<EndpointDataSource> sources) =>
//{
//    var sb = new System.Text.StringBuilder();
//    sb.AppendLine("=== Registered endpoints ===");
//    foreach (var ep in sources.SelectMany(s => s.Endpoints))
//    {
//        sb.AppendLine(ep.DisplayName);
//        if (ep is RouteEndpoint re && re.RoutePattern is not null)
//            sb.AppendLine("  → " + re.RoutePattern.RawText);
//    }
//    return sb.ToString();
//});

//// Ветка middleware (НЕ endpoints)
//app.Map("/home", home =>
//{
//    home.Map("/index", app2 => app2.Run(async c => await c.Response.WriteAsync("Index Page")));
//    home.Map("/about", app2 => app2.Run(async c => await c.Response.WriteAsync("About Page")));
//    home.Run(async c => await c.Response.WriteAsync("Home Page"));
//});

//// Хвост — добавляем текст ПОСЛЕ исполнения обработчика (один раз)
//app.Use(async (context, next) =>
//{
//    await next();
//    await context.Response.WriteAsync("  TestDate1  ");
//});

app.Run();






