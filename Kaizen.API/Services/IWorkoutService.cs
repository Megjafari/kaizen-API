using Kaizen.API.Models;

namespace Kaizen.API.Services;

public interface IWorkoutService
{
    Task<List<WorkoutTemplate>> GetTemplatesAsync();
    Task<WorkoutTemplate?> GetTemplateAsync(int id);
    Task<List<WorkoutLog>> GetLogsAsync(string userId, DateTime? from, DateTime? to);
    Task<WorkoutLog?> GetLogAsync(string userId, int id);
    Task<WorkoutLog> CreateLogAsync(string userId, WorkoutLog log);
    Task<bool> DeleteLogAsync(string userId, int id);
}