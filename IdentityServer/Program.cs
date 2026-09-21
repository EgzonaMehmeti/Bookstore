using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services
    .AddIdentityServer(options =>
    {
        options.UserInteraction.LoginUrl = "/Account/Login";
    })
    .AddDeveloperSigningCredential()
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.Clients);

var app = builder.Build();

app.UseIdentityServer();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", () => "IdentityServer is running.");

app.Run();