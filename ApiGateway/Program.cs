
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("https://localhost:44307");

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-super-secret-key-change-me";

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });


builder.Services.AddAuthorization();
builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => Results.Ok("API Gateway is running"));

app.Use(async (ctx, next) =>
{
    // Если клиент не отправил X-Request-Id, создаём его
    const string headerName = "X-Request-Id";
    if (!ctx.Request.Headers.ContainsKey(headerName))
    {
        ctx.Request.Headers[headerName] = Guid.NewGuid().ToString("N");
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();
app.Run();
