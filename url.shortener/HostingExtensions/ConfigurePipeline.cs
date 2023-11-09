using AspNetCoreRateLimit;

namespace url.shortener.HostingExtensions;

public static class ConfigurePipeline
{
    public static WebApplication PipelineConfigure(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseMiddleware<ClientRateLimitMiddleware>();

        app.UseHttpsRedirection();

        //app.MinimalApisDefinition();

        return app;
    }
}