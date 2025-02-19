using NestAPIGateway;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddOcelot().AddConsul();
builder.Configuration.AddJsonFile
    ("Ocelot.json",
    optional: false,
    reloadOnChange: true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
await app.RegisterWithConsul(app.Lifetime);
app.UseWhen(context => !context.Request.Path.StartsWithSegments("/health"), subApp =>
{
    subApp.UseOcelot().Wait();
});
await app.RunAsync();