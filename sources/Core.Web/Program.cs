using Core.Web.StartUp;

var builder = WebApplication.CreateBuilder(args);

ServiceRegistration.Register(builder);

var app = builder.Build();

Configuration.Configure(app);

app.Run();
