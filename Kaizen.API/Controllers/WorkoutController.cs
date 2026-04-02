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
public class WorkoutController : ControllerBase
{
    private readonly KaizenDbContext _context;

    public WorkoutController(KaizenDbContext context)
    {
        _context = context;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    //TEMPLATES (publika)

    [HttpGet("templates")]
    [AllowAnonymous]
    public async Task<ActionResult<List<WorkoutTemplate>>> GetTemplates()
    {
        return await _context.WorkoutTemplates
            .Include(t => t.Exercises)
            .ToListAsync();
    }

    [HttpGet("templates/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<WorkoutTemplate>> GetTemplate(int id)
    {
        var template = await _context.WorkoutTemplates
            .Include(t => t.Exercises)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (template == null)
            return NotFound();

        return template;
    }

    //WORKOUT LOGS (användarens)

    [HttpGet("logs")]
    public async Task<ActionResult<List<WorkoutLog>>> GetLogs([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var query = _context.WorkoutLogs
            .Include(w => w.Exercises)
            .Where(w => w.UserId == GetUserId());

        if (from.HasValue)
            query = query.Where(w => w.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(w => w.Date <= to.Value);

        return await query.OrderByDescending(w => w.Date).ToListAsync();
    }

    [HttpGet("logs/{id}")]
    public async Task<ActionResult<WorkoutLog>> GetLog(int id)
    {
        var log = await _context.WorkoutLogs
            .Include(w => w.Exercises)
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == GetUserId());

        if (log == null)
            return NotFound();

        return log;
    }

    [HttpPost("logs")]
    public async Task<ActionResult<WorkoutLog>> CreateLog(WorkoutLog log)
    {
        log.UserId = GetUserId();

        _context.WorkoutLogs.Add(log);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLog), new { id = log.Id }, log);
    }

    [HttpDelete("logs/{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var log = await _context.WorkoutLogs
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == GetUserId());

        if (log == null)
            return NotFound();

        _context.WorkoutLogs.Remove(log);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}