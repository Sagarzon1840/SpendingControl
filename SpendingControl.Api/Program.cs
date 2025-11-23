using SpendingControl.Api; // namespace for Startup

var builder = WebApplication.CreateBuilder(args);
// Build host with Startup (mirror LambdaEntryPoint behavior)
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, app.Environment);

app.Run();