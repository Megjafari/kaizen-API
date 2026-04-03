using Kaizen.API.Models;

namespace Kaizen.API.Services;

public interface IFoodService
{
    Task<List<Ingredient>> SearchIngredientsAsync(string? query);
    Task<Ingredient?> GetIngredientAsync(int id);
    Task<List<FoodLog>> GetLogsAsync(string userId, DateTime? date);
    Task<FoodLog> CreateLogAsync(string userId, FoodLog log);
    Task<bool> DeleteLogAsync(string userId, int id);
    Task<DailySummary> GetDailySummaryAsync(string userId, DateTime date);
}

public class DailySummary
{
    public DateTime Date { get; set; }
    public decimal TotalCalories { get; set; }
    public decimal TotalProtein { get; set; }
    public decimal TotalCarbs { get; set; }
    public decimal TotalFat { get; set; }
    public int Items { get; set; }
}