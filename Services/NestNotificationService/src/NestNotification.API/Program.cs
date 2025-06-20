// Nest Notification API Program.cs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNotificationServices();

builder.Services.AddHostedService<EmailBackgroundService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(Configurations.GetConfiguration<ServiceDiscovery>().Consul.ServiceRegistration.Port);
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
app.UseRabbitMqEventBus();

await app.RunAsync();