using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaizen.API.Models;
using Kaizen.API.Services;
using System.Security.Claims;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("templates")]
    [AllowAnonymous]
    public async Task<ActionResult<List<WorkoutTemplate>>> GetTemplates()
    {
        return await _workoutService.GetTemplatesAsync();
    }

    [HttpGet("templates/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<WorkoutTemplate>> GetTemplate(int id)
    {
        var template = await _workoutService.GetTemplateAsync(id);

        if (template == null)
            return NotFound();

        return template;
    }

    [HttpGet("logs")]
    public async Task<ActionResult<List<WorkoutLog>>> GetLogs([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return await _workoutService.GetLogsAsync(GetUserId(), from, to);
    }

    [HttpGet("logs/{id}")]
    public async Task<ActionResult<WorkoutLog>> GetLog(int id)
    {
        var log = await _workoutService.GetLogAsync(GetUserId(), id);

        if (log == null)
            return NotFound();

        return log;
    }

    [HttpPost("logs")]
    public async Task<ActionResult<WorkoutLog>> CreateLog(WorkoutLog log)
    {
        var created = await _workoutService.CreateLogAsync(GetUserId(), log);
        return CreatedAtAction(nameof(GetLog), new { id = created.Id }, created);
    }

    [HttpDelete("logs/{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var deleted = await _workoutService.DeleteLogAsync(GetUserId(), id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}