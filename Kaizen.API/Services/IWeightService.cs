using Kaizen.API.Models;

namespace Kaizen.API.Services;

public interface IWeightService
{
    Task<List<WeightLog>> GetLogsAsync(string userId, DateTime? from, DateTime? to);
    Task<WeightLog> LogWeightAsync(string userId, WeightLog log);
    Task<bool> DeleteLogAsync(string userId, int id);
}