using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaizen.API.Models;
using Kaizen.API.Services;
using System.Security.Claims;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeightController : ControllerBase
{
    private readonly IWeightService _weightService;

    public WeightController(IWeightService weightService)
    {
        _weightService = weightService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<WeightLog>>> GetLogs([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return await _weightService.GetLogsAsync(GetUserId(), from, to);
    }

    [HttpPost]
    public async Task<ActionResult<WeightLog>> CreateLog(WeightLog log)
    {
        var result = await _weightService.LogWeightAsync(GetUserId(), log);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLog(int id, WeightLog log)
    {
        var updated = await _weightService.UpdateLogAsync(GetUserId(), id, log);
        
        if (updated == null)
            return NotFound();
        
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var deleted = await _weightService.DeleteLogAsync(GetUserId(), id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}