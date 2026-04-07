using Kaizen.API.Models;

namespace Kaizen.API.Services;

public interface IProgressService
{
    Task<List<ProgressPhoto>> GetPhotosAsync(string userId);
    Task<ProgressPhoto> AddPhotoAsync(string userId, ProgressPhoto photo);
    Task<bool> DeletePhotoAsync(string userId, int id);
}