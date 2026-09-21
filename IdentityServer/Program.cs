using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.Clients);

var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "IdentityServer is running.");

app.Run();