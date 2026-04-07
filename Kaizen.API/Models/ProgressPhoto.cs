namespace Kaizen.API.Models;

public class ProgressPhoto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Note { get; set; }
    public decimal? Weight { get; set; }
}