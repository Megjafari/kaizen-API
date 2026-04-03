using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaizen.API.Models;
using Kaizen.API.Services;
using System.Security.Claims;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FoodController : ControllerBase
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("ingredients")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Ingredient>>> SearchIngredients([FromQuery] string? q)
    {
        return await _foodService.SearchIngredientsAsync(q);
    }

    [HttpGet("ingredients/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Ingredient>> GetIngredient(int id)
    {
        var ingredient = await _foodService.GetIngredientAsync(id);

        if (ingredient == null)
            return NotFound();

        return ingredient;
    }

    [HttpGet("logs")]
    public async Task<ActionResult<List<FoodLog>>> GetLogs([FromQuery] DateTime? date)
    {
        return await _foodService.GetLogsAsync(GetUserId(), date);
    }

    [HttpPost("logs")]
    public async Task<ActionResult<FoodLog>> CreateLog(FoodLog log)
    {
        var created = await _foodService.CreateLogAsync(GetUserId(), log);
        return CreatedAtAction(nameof(GetLogs), created);
    }

    [HttpDelete("logs/{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var deleted = await _foodService.DeleteLogAsync(GetUserId(), id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DailySummary>> GetDailySummary([FromQuery] DateTime date)
    {
        return await _foodService.GetDailySummaryAsync(GetUserId(), date);
    }
}