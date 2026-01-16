
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Читаем секрет для подписи токенов из конфигурации
var jwtKey = builder.Configuration["Jwt:Key"] ?? "dev-super-secret-key-change-me";
var issuer = "MicroservicesDemo";
var audience = "microservices.api";

var app = builder.Build();

// DEV endpoint: выдать access_token
// POST http://localhost:7001/token  { "username": "mikita" }

app.MapGet("/", () => Results.Ok("DevAuth is running. Use POST /token to get a JWT."));

app.MapPost("/token", (string? username) =>
{
    var claims = new List<Claim> {
        new(JwtRegisteredClaimNames.Sub, username ?? "dev-user")
        // при желании добавим роли/permissions позже для авторизации
    };

    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        //issuer: issuer,
        //audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds
    );

    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { access_token = accessToken, token_type = "Bearer", expires_in = 3600 });
});

app.Run();
