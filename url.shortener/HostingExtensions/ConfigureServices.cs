using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using url.shortener.OptionModels;
using url.shortener.UrlDatabaseContext;

namespace url.shortener.HostingExtensions;

public static class ConfigureServices
{
    public static WebApplication? ServicesConfigure(this WebApplicationBuilder builder)
    {
        //if(builder.Environment.IsDevelopment())
        //{
        //    builder.Services.AddDbContext<UrlDbContext>(opt => opt.UseInMemoryDatabase("UrlDatabase"));
        //}
        //else
        //{
        builder.Services.AddDbContext<UrlDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings:SqlServer").Value));
        //}

        builder.Services.ConfigureAppOptions(builder);

        builder.Services.Configure<ClientRateLimitOptions>(options =>
        {
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1m",
                    Limit = 10,
                }
            };
        });

        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        builder.Services.AddMemoryCache();

        return builder.Build();
    }

}
