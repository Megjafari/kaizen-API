using Microsoft.EntityFrameworkCore;
using Kaizen.API.Data;
using Kaizen.API.Models;
using Kaizen.API.DTOs;

namespace Kaizen.API.Services;

public class WorkoutService : IWorkoutService
{
    private readonly KaizenDbContext _context;

    public WorkoutService(KaizenDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkoutTemplate>> GetTemplatesAsync()
    {
        return await _context.WorkoutTemplates
            .Include(t => t.Exercises)
            .ToListAsync();
    }

    public async Task<WorkoutTemplate?> GetTemplateAsync(int id)
    {
        return await _context.WorkoutTemplates
            .Include(t => t.Exercises)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<WorkoutLog>> GetLogsAsync(string userId, DateTime? from, DateTime? to)
    {
        var query = _context.WorkoutLogs
            .Include(w => w.Exercises)
            .Where(w => w.UserId == userId);

        if (from.HasValue)
            query = query.Where(w => w.Date >= from.Value);

        if (to.HasValue)
            query = query.Where(w => w.Date <= to.Value);

        return await query.OrderByDescending(w => w.Date).ToListAsync();
    }

    public async Task<WorkoutLog?> GetLogAsync(string userId, int id)
    {
        return await _context.WorkoutLogs
            .Include(w => w.Exercises)
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
    }

    public async Task<WorkoutLog> CreateLogAsync(string userId, CreateWorkoutLogDto dto)
    {
        var log = new WorkoutLog
        {
            UserId = userId,
            Date = dto.Date,
            Name = dto.Name,
            Notes = dto.Notes,
            Exercises = dto.Exercises.Select(e => new ExerciseLog
            {
                ExerciseName = e.ExerciseName,
                Sets = e.Sets,
                Reps = e.Reps,
                Weight = e.Weight
            }).ToList()
        };

        _context.WorkoutLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<bool> DeleteLogAsync(string userId, int id)
    {
        var log = await _context.WorkoutLogs
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

        if (log == null)
            return false;

        _context.WorkoutLogs.Remove(log);
        await _context.SaveChangesAsync();
        return true;
    }
}