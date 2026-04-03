using Microsoft.EntityFrameworkCore;
using Kaizen.API.Data;

namespace Kaizen.API.Services;

public class WeeklySummaryService : IWeeklySummaryService
{
    private readonly KaizenDbContext _context;

    public WeeklySummaryService(KaizenDbContext context)
    {
        _context = context;
    }

    public async Task<WeeklySummary> GetSummaryAsync(string userId, DateTime? weekOf)
    {
        var date = weekOf ?? DateTime.UtcNow;
        var monday = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
        var sunday = monday.AddDays(6);

        var workouts = await _context.WorkoutLogs
            .Where(w => w.UserId == userId && w.Date >= monday && w.Date <= sunday)
            .ToListAsync();

        var workoutDays = workouts.Select(w => w.Date.Date).Distinct().Count();

        var foodLogs = await _context.FoodLogs
            .Include(f => f.Ingredient)
            .Where(f => f.UserId == userId && f.Date >= monday && f.Date <= sunday)
            .ToListAsync();

        var caloriesByDay = foodLogs
            .GroupBy(f => f.Date.Date)
            .Select(g => g.Sum(f => f.Ingredient.Calories * f.AmountGrams / 100))
            .ToList();

        var avgCalories = caloriesByDay.Any() ? caloriesByDay.Average() : 0;

        var weights = await _context.WeightLogs
            .Where(w => w.UserId == userId && w.Date >= monday && w.Date <= sunday)
            .OrderBy(w => w.Date)
            .ToListAsync();

        decimal? weightChange = null;
        if (weights.Count >= 2)
            weightChange = weights.Last().Weight - weights.First().Weight;

        return new WeeklySummary
        {
            WeekOf = monday.Date,
            WorkoutDays = workoutDays,
            TotalWorkouts = workouts.Count,
            AvgDailyCalories = Math.Round(avgCalories, 0),
            DaysTracked = caloriesByDay.Count,
            StartWeight = weights.FirstOrDefault()?.Weight,
            EndWeight = weights.LastOrDefault()?.Weight,
            WeightChange = weightChange
        };
    }
}