// Nest Notification API Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNotificationServices();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(NestNotification.API.Configuration.GetConfiguratinValue<int>("Consul", "ConsulClientRegister", "ServerPort"));
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCustomExceptionHandler();

await app.RegisterWithConsul(app.Lifetime);

app.Run();