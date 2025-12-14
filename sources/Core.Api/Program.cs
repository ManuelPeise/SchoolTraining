using Core.Api.Bundels;

const string CorsPolicy = "CorsPolicy";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ServiceRegistration.RegisterServices(builder, CorsPolicy);

var app = builder.Build();

// Configure the HTTP request pipeline.
await AppConfiguration.Configure(app, CorsPolicy);

app.Run();
