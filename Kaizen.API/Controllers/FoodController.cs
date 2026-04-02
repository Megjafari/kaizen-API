using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kaizen.API.Data;
using Kaizen.API.Models;
using System.Security.Claims;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodController : ControllerBase
{
    private readonly KaizenDbContext _context;

    public FoodController(KaizenDbContext context)
    {
        _context = context;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    //  INGREDIENTS (sökbara) 

    [HttpGet("ingredients")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Ingredient>>> SearchIngredients([FromQuery] string? q)
    {
        var query = _context.Ingredients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(i => i.Name.ToLower().Contains(q.ToLower()));

        return await query.Take(20).ToListAsync();
    }

    [HttpGet("ingredients/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Ingredient>> GetIngredient(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);

        if (ingredient == null)
            return NotFound();

        return ingredient;
    }

    // FOOD LOGS (användarens) 

    [HttpGet("logs")]
    public async Task<ActionResult<List<FoodLog>>> GetLogs([FromQuery] DateTime? date)
    {
        var query = _context.FoodLogs
            .Include(f => f.Ingredient)
            .Where(f => f.UserId == GetUserId());

        if (date.HasValue)
            query = query.Where(f => f.Date.Date == date.Value.Date);

        return await query.OrderByDescending(f => f.Date).ToListAsync();
    }

    [HttpPost("logs")]
    public async Task<ActionResult<FoodLog>> CreateLog(FoodLog log)
    {
        log.UserId = GetUserId();

        _context.FoodLogs.Add(log);
        await _context.SaveChangesAsync();

        await _context.Entry(log).Reference(f => f.Ingredient).LoadAsync();

        return CreatedAtAction(nameof(GetLogs), log);
    }

    [HttpDelete("logs/{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var log = await _context.FoodLogs
            .FirstOrDefaultAsync(f => f.Id == id && f.UserId == GetUserId());

        if (log == null)
            return NotFound();

        _context.FoodLogs.Remove(log);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DAILY SUMMARY

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetDailySummary([FromQuery] DateTime date)
    {
        var logs = await _context.FoodLogs
            .Include(f => f.Ingredient)
            .Where(f => f.UserId == GetUserId() && f.Date.Date == date.Date)
            .ToListAsync();

        var summary = new
        {
            Date = date.Date,
            TotalCalories = logs.Sum(f => f.Ingredient.Calories * f.AmountGrams / 100),
            TotalProtein = logs.Sum(f => f.Ingredient.Protein * f.AmountGrams / 100),
            TotalCarbs = logs.Sum(f => f.Ingredient.Carbs * f.AmountGrams / 100),
            TotalFat = logs.Sum(f => f.Ingredient.Fat * f.AmountGrams / 100),
            Items = logs.Count
        };

        return summary;
    }
}