var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(Configurations.GetConfiguration<ServiceDiscovery>().Consul.ServiceRegistration.Port);
    });
}

builder.Services.AddServices();
builder.Services.AddOcelot().AddConsul();
builder.Configuration.AddJsonFile("Ocelot.json",
    false,
    true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

await app.RegisterWithConsul(app.Lifetime);

app.UseWhen(context => !context.Request.Path.StartsWithSegments("/health"), subApp => { subApp.UseOcelot().Wait(); });
await app.RunAsync();