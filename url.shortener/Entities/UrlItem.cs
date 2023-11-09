namespace Url.Shorter.Entities;

public class UrlItem
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public byte ExpireTimeInDay { get; set; }
    public VisitHistory VisitHistory { get; set; }
}
