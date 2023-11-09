namespace url.shortener.GeneralModels;

public class VisitHistoryReponseModel
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public DateTime Created { get; set; }
    public long VisitCount { get; set; }
    public bool IsExpired { get; set; }
}