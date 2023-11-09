namespace url.shortener.Entities;

public class VisitHistory
{
    public int Id { get; set; }
    public int UrlItemId { get; set; }
    public UrlItem UrlItem { get; set; }
    public long VisitCount { get; set; }
}
