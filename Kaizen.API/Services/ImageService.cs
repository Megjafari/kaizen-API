using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Kaizen.API.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IConfiguration config)
    {
        var account = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(string base64Image)
    {
        // Remove data:image/...;base64, prefix if present
        var base64Data = base64Image.Contains(",") 
            ? base64Image.Split(",")[1] 
            : base64Image;

        var bytes = Convert.FromBase64String(base64Data);
        using var stream = new MemoryStream(bytes);

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription("profile", stream),
            Folder = "kaizen/profiles",
            Transformation = new Transformation().Width(200).Height(200).Crop("fill").Gravity("face")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);
        return result.SecureUrl.ToString();
    }

    public async Task<bool> DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return true;

        // Extract public ID from URL
        var uri = new Uri(imageUrl);
        var path = uri.AbsolutePath;
        var publicId = path.Split("/upload/")[1];
        publicId = publicId.Substring(publicId.IndexOf("/") + 1);
        publicId = publicId.Substring(0, publicId.LastIndexOf("."));

        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok";
    }
}