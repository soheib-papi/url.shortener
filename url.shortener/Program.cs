using url.shortener.HostingExtensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.ServicesConfigure()?.PipelineConfigure();

app.Run();
