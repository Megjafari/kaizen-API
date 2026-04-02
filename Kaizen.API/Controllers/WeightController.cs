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
public class WeightController : ControllerBase
{
    private readonly KaizenDbContext _context;

    public WeightController(KaizenDbContext context)
    {
        _context = context;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<WeightLog>>> GetLogs([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var query = _context.WeightLogs
            .Where(w => w.UserId == GetUserId());

        if (from.HasValue)
            query = query.Where(w => w.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(w => w.Date <= to.Value);

        return await query.OrderByDescending(w => w.Date).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<WeightLog>> CreateLog(WeightLog log)
    {
        log.UserId = GetUserId();

        // En vikt per dag - ersätt om finns
        var existing = await _context.WeightLogs
            .FirstOrDefaultAsync(w => w.UserId == log.UserId && w.Date.Date == log.Date.Date);

        if (existing != null)
        {
            existing.Weight = log.Weight;
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        _context.WeightLogs.Add(log);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLogs), log);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLog(int id)
    {
        var log = await _context.WeightLogs
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == GetUserId());

        if (log == null)
            return NotFound();

        _context.WeightLogs.Remove(log);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}