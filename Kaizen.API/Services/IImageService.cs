namespace Kaizen.API.Services;

public interface IImageService
{
    Task<string> UploadImageAsync(string base64Image);
    Task<bool> DeleteImageAsync(string imageUrl);
}