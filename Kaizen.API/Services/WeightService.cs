using Microsoft.EntityFrameworkCore;
using Kaizen.API.Data;
using Kaizen.API.Models;

namespace Kaizen.API.Services;

public class WeightService : IWeightService
{
    private readonly KaizenDbContext _context;

    public WeightService(KaizenDbContext context)
    {
        _context = context;
    }

    public async Task<List<WeightLog>> GetLogsAsync(string userId, DateTime? from, DateTime? to)
    {
        var query = _context.WeightLogs
            .Where(w => w.UserId == userId);

        if (from.HasValue)
            query = query.Where(w => w.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(w => w.Date <= to.Value);

        return await query.OrderByDescending(w => w.Date).ToListAsync();
    }

    public async Task<WeightLog> LogWeightAsync(string userId, WeightLog log)
    {
        log.UserId = userId;

        var existing = await _context.WeightLogs
            .FirstOrDefaultAsync(w => w.UserId == userId && w.Date.Date == log.Date.Date);

        if (existing != null)
        {
            existing.Weight = log.Weight;
            await _context.SaveChangesAsync();
            return existing;
        }

        _context.WeightLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<bool> DeleteLogAsync(string userId, int id)
    {
        var log = await _context.WeightLogs
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

        if (log == null)
            return false;

        _context.WeightLogs.Remove(log);
        await _context.SaveChangesAsync();
        return true;
    }
}