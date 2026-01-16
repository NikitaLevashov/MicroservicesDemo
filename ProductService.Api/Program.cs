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




using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductService.Application.Interfaces;
using ProductService.Application.Services;
using ProductService.Infrastructure.Persistence;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(x =>
{
    // Consumers здесь не нужны — мы только публикуем события
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Простые ретраи на случай кратких сбоев
        cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(2)));

        // Красивые имена (префикс по сервису)
        cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("product-service", false));
    });
});

//builder.Services.AddMassTransitHostedService(true);


// ==============================
// 1) СЕРВИСЫ (Dependency Injection)
// ==============================

// База данных
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Бизнес-логика
builder.Services.AddTransient<IProductService, ProductServiceApp>();
builder.Services.AddSingleton<ClaimsPrincipal>();
builder.Services.AddMemoryCache();
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

var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-super-secret-key-change-me";

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });


builder.Services.AddAuthorization();


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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();













////////////////////////////////////
///



//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authentication;

//var builder = WebApplication.CreateBuilder();

//// условная бд с пользователями
//var people = new List<Person>
//{
//    new Person("tom@gmail.com", "12345"),
//    new Person("bob@gmail.com", "55555")
//};
//// аутентификация с помощью куки
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options => options.LoginPath = "/login");
//builder.Services.AddAuthorization();

//var app = builder.Build();

//app.UseAuthentication();   // добавление middleware аутентификации 
//app.UseAuthorization();   // добавление middleware авторизации 

//app.MapGet("/login", async (HttpContext context) =>
//{
//    context.Response.ContentType = "text/html; charset=utf-8";
//    // html-форма для ввода логина/пароля
//    string loginForm = @"<!DOCTYPE html>
//    <html>
//    <head>
//        <meta charset='utf-8' />
//        <title>METANIT.COM</title>
//    </head>
//    <body>
//        <h2>Login Form</h2>
//        <form method='post'>
//            <p>
//                <label>Email</label><br />
//                <input name='email' />
//            </p>
//            <p>
//                <label>Password</label><br />
//                <input type='password' name='password' />
//            </p>
//            <input type='submit' value='Login' />
//        </form>
//    </body>
//    </html>";
//    await context.Response.WriteAsync(loginForm);
//});

//app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
//{
//    // получаем из формы email и пароль
//    var form = context.Request.Form;
//    // если email и/или пароль не установлены, посылаем статусный код ошибки 400
//    if (!form.ContainsKey("email") || !form.ContainsKey("password"))
//        return Results.BadRequest("Email и/или пароль не установлены");

//    string email = form["email"];
//    string password = form["password"];

//    // находим пользователя 
//    Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
//    // если пользователь не найден, отправляем статусный код 401
//    if (person is null) return Results.Unauthorized();

//    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
//    // создаем объект ClaimsIdentity
//    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

//    // установка аутентификационных куки
//    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
//    return Results.Redirect(returnUrl ?? "/");
//});

//app.MapGet("/logout", async (HttpContext context) =>
//{
//    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//    return Results.Redirect("/login");
//});

//app.Run();

//record class Person(string Email, string Password);






