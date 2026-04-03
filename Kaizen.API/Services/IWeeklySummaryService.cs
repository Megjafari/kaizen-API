namespace Kaizen.API.Services;

public interface IWeeklySummaryService
{
    Task<WeeklySummary> GetSummaryAsync(string userId, DateTime? weekOf);
}

public class WeeklySummary
{
    public DateTime WeekOf { get; set; }
    public int WorkoutDays { get; set; }
    public int TotalWorkouts { get; set; }
    public decimal AvgDailyCalories { get; set; }
    public int DaysTracked { get; set; }
    public decimal? StartWeight { get; set; }
    public decimal? EndWeight { get; set; }
    public decimal? WeightChange { get; set; }
}