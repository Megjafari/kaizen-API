using Microsoft.EntityFrameworkCore;
using Kaizen.API.Data;
using Kaizen.API.Models;

namespace Kaizen.API.Services;

public class ProfileService : IProfileService
{
    private readonly KaizenDbContext _context;

    public ProfileService(KaizenDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetProfileAsync(string userId)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<UserProfile> CreateProfileAsync(string userId, UserProfile profile)
    {
        profile.UserId = userId;
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<UserProfile?> UpdateProfileAsync(string userId, UserProfile updated)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (profile == null)
            return null;

        profile.Height = updated.Height;
        profile.Weight = updated.Weight;
        profile.Age = updated.Age;
        profile.Gender = updated.Gender;
        profile.Goal = updated.Goal;

        await _context.SaveChangesAsync();
        return profile;
    }
    public async Task<bool> DeleteAccountAsync(string userId)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        // Delete all user data
        var workoutLogs = _context.WorkoutLogs.Where(w => w.UserId == userId);
        var foodLogs = _context.FoodLogs.Where(f => f.UserId == userId);
        var weightLogs = _context.WeightLogs.Where(w => w.UserId == userId);

        _context.WorkoutLogs.RemoveRange(workoutLogs);
        _context.FoodLogs.RemoveRange(foodLogs);
        _context.WeightLogs.RemoveRange(weightLogs);

        if (profile != null)
            _context.UserProfiles.Remove(profile);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task UpdateProfileImageAsync(string userId, string imageUrl)
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile != null)
        {
            profile.ProfileImageUrl = imageUrl;
            await _context.SaveChangesAsync();
        }
    }
}