using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using url.shortener.Entities;
using url.shortener.GeneralModels;
using url.shortener.OptionModels;
using url.shortener.UrlDatabaseContext;

namespace url.shortener.HostingExtensions;

public static class MinimalApis
{
    public static WebApplication MinimalApisDefinition(this WebApplication app)
    {
        /// <summary>
        /// Desc: این متد یک آدرس وب می گیرد و فرم کوتاه شده آن را بی می گرداند
        /// </summary>
        app.MapPost("/urlitems", async (HttpContext httpContext, string url, CancellationToken cancellationToken) =>
        {
            Uri uriResult;
            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (!isValidUrl) return BaseResponse<string>.Failure("Url is not valid.");

            var db = httpContext.RequestServices.GetRequiredService<UrlDbContext>();

            var linkExpiteTimeInDay = httpContext.RequestServices.GetRequiredService<IOptionsMonitor<UrlExpireTimeInDayOptions>>();

            var urlItem = new UrlItem()
            {
                OriginalUrl = url,
                ExpireTimeInDay = linkExpiteTimeInDay.CurrentValue.Value
            };
            db.UrlItems.Add(urlItem);
            await db.SaveChangesAsync(cancellationToken);

            db.VisitHistories.Add(new VisitHistory() { UrlItemId = urlItem.Id, VisitCount = 0 });
            await db.SaveChangesAsync(cancellationToken);

            return BaseResponse<string>.Success($"/urlitems/{urlItem.Id}");
        });

        /// <summary>
        /// Desc: این متد یک آدرس کوتاه شده می گیرد و آدرس اصلی آن را برمی گرداند
        /// </summary>
        app.MapGet("/urlitems/{id}", async (HttpContext httpContext, int id, CancellationToken cancellationToken) =>
        {
            var db = httpContext.RequestServices.GetRequiredService<UrlDbContext>();

            var urlItem = await db.UrlItems.FirstOrDefaultAsync(f => f.Id == id &&
                f.Created.AddDays(f.ExpireTimeInDay) >= DateTime.UtcNow, cancellationToken);

            if (urlItem == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                return BaseResponse<string>.Failure("Url not found.");
            }

            var visitHistory = await db.VisitHistories.FirstOrDefaultAsync(f => f.UrlItemId == id, cancellationToken);

            if (visitHistory is null) return BaseResponse<string>.Failure("Url not found.");

            visitHistory.VisitCount++;
            await db.SaveChangesAsync(cancellationToken);

            return BaseResponse<string>.Success(urlItem.OriginalUrl);
        });

        /// <summary>
        /// Desc: این متد شناسه یک آدرس کوتاه را می گیرد و تعداد بازدید آن را برمی گرداند
        /// </summary>
        app.MapGet("/urlitems/visits/{id}", async (HttpContext httpContext, int id, CancellationToken cancellationToken) =>
        {
            var db = httpContext.RequestServices.GetRequiredService<UrlDbContext>();

            var visitHistory = await GetVisitHistory(db, id, cancellationToken); ;

            if (visitHistory == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                return BaseResponse<long>.Failure("Url not found.");
            }

            return BaseResponse<long>.Success(visitHistory.VisitCount);
        });

        /// <summary>
        /// Desc: این متد لیست آدرسهایی که منقضی نشده اند و تعداد بازدید آنها را برمی گرداند
        /// </summary>
        app.MapGet("/urlitems/summary/", async (HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var db = httpContext.RequestServices.GetRequiredService<UrlDbContext>();

            var visitHistory = await GetAllSummary(db, cancellationToken);

            if (visitHistory == null || visitHistory.Count == 0)
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                return Results.NotFound(BaseResponse<string>.Failure("Url not found."));
            }

            return Results.Ok(BaseResponse<List<VisitHistoryReponseModel>>.Success(visitHistory));
        });

        return app;
    }

    private static async Task<VisitHistory?> GetVisitHistory(UrlDbContext db, int id, CancellationToken cancellationToken)
    {
        return await db.VisitHistories.FirstOrDefaultAsync(f => f.UrlItemId == id, cancellationToken);
    }

    private static async Task<List<VisitHistoryReponseModel>?> GetAllSummary(UrlDbContext db, CancellationToken cancellationToken)
    {
        return await db.UrlItems
            .Include(i => i.VisitHistory)
            .Where(f => f.Created.AddDays(f.ExpireTimeInDay) >= DateTime.UtcNow)
            .Select(f => new VisitHistoryReponseModel()
            {
                Id = f.Id,
                OriginalUrl = f.OriginalUrl,
                Created = f.Created,
                VisitCount = f.VisitHistory.VisitCount,
                IsExpired = !(f.Created.AddDays(f.ExpireTimeInDay) >= DateTime.UtcNow)
            })
            .ToListAsync(cancellationToken);
    }

}