var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
//builder.Services.AddRabbitMqEventBus();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
//app.UseRabbitMqEventBus();

app.MapControllers();

app.Run();