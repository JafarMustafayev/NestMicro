using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStorageServices();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 1024 * 1024 * 500; // 500MB
});

// 🔐 JWT Authentication əlavə et
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer",
        options =>
        {
            options.Authority = "https://localhost:5181"; // AuthService URL-i (token verən servis)
            options.RequireHttpsMetadata = false;
            options.Audience = "gateway"; // Token-dəki "aud" field (Ocelot-da AllowedScopes ilə uyğun olmalı)
        });

// 🔐 Authorization
builder.Services.AddAuthorization();

// 🔎 Swagger üçün JWT konfiqurasiyası
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Gateway", Version = "v1" });

    c.AddSecurityDefinition("Bearer",
        new()
        {
            Description = "JWT istifadə üçün 'Bearer {token}' formatında yaz",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
app.UseRouting();
app.UseAuthentication(); // <-- vacibdir
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();