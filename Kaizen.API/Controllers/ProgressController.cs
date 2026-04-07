using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaizen.API.Models;
using Kaizen.API.Services;
using System.Security.Claims;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;
    private readonly IImageService _imageService;

    public ProgressController(IProgressService progressService, IImageService imageService)
    {
        _progressService = progressService;
        _imageService = imageService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<ProgressPhoto>>> GetPhotos()
    {
        return await _progressService.GetPhotosAsync(GetUserId());
    }

    [HttpPost]
    public async Task<ActionResult<ProgressPhoto>> AddPhoto([FromBody] ProgressPhotoDto dto)
    {
        var imageUrl = await _imageService.UploadImageAsync(dto.Base64Image);

        var photo = new ProgressPhoto
        {
            Date = dto.Date,
            ImageUrl = imageUrl,
            Note = dto.Note
        };

        var result = await _progressService.AddPhotoAsync(GetUserId(), photo);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(int id)
    {
        var deleted = await _progressService.DeletePhotoAsync(GetUserId(), id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

public class ProgressPhotoDto
{
    public DateTime Date { get; set; }
    public string Base64Image { get; set; } = string.Empty;
    public string? Note { get; set; }
}