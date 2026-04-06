namespace Kaizen.API.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty; // Auth0 sub
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty; // lose/maintain/gain
    public string? ProfileImageUrl { get; set; }
}