using Kaizen.API.Models;

namespace Kaizen.API.Services;

public interface IProfileService
{
    Task<UserProfile?> GetProfileAsync(string userId);
    Task<UserProfile> CreateProfileAsync(string userId, UserProfile profile);
    Task<UserProfile?> UpdateProfileAsync(string userId, UserProfile updated);
    Task<bool> DeleteAccountAsync(string userId);
    Task UpdateProfileImageAsync(string userId, string imageUrl);
}