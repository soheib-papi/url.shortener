namespace url.shortener.OptionModels;

public static class OptionsConfiguration
{
    public static void ConfigureAppOptions(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.Configure<UrlExpireTimeInDayOptions>(builder.Configuration.GetSection(nameof(UrlExpireTimeInDayOptions)));
    }
}