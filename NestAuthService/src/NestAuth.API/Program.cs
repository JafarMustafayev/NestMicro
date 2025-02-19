//Authservice API Program.cs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthServices();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();

    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(NestAuth.API.Configuration.GetConfiguratinValue<int>("Consul", "ConsulClientRegister", "ServerPort"));
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.RegisterWithConsul(app.Lifetime);

await app.RunAsync();