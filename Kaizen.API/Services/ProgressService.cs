using Microsoft.EntityFrameworkCore;
using Kaizen.API.Data;
using Kaizen.API.Models;

namespace Kaizen.API.Services;

public class ProgressService : IProgressService
{
    private readonly KaizenDbContext _context;

    public ProgressService(KaizenDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProgressPhoto>> GetPhotosAsync(string userId)
    {
        return await _context.ProgressPhotos
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.Date)
            .ToListAsync();
    }

    public async Task<ProgressPhoto> AddPhotoAsync(string userId, ProgressPhoto photo)
    {
        photo.UserId = userId;
        photo.Date = DateTime.SpecifyKind(photo.Date, DateTimeKind.Utc);

        // Hämta dagens vikt om den finns
        var todaysWeight = await _context.WeightLogs
            .Where(w => w.UserId == userId && w.Date.Date == photo.Date.Date)
            .FirstOrDefaultAsync();
        
        if (todaysWeight != null)
        {
            photo.Weight = todaysWeight.Weight;
        }

        _context.ProgressPhotos.Add(photo);
        await _context.SaveChangesAsync();
        return photo;
    }

    public async Task<bool> DeletePhotoAsync(string userId, int id)
    {
        var photo = await _context.ProgressPhotos
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (photo == null)
            return false;

        _context.ProgressPhotos.Remove(photo);
        await _context.SaveChangesAsync();
        return true;
    }
}