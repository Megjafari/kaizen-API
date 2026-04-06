using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaizen.API.Models;
using Kaizen.API.Services;
using System.Security.Claims;
using Kaizen.API.DTOs;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    private readonly IImageService _imageService;

    public ProfileController(IProfileService profileService, IImageService imageService)
    {
        _profileService = profileService;
        _imageService = imageService;   
    }

    [HttpPost("image")]
    public async Task<ActionResult<string>> UploadProfileImage([FromBody] ImageUploadDto dto)
    {
        var imageUrl = await _imageService.UploadImageAsync(dto.Base64Image);
        await _profileService.UpdateProfileImageAsync(GetUserId(), imageUrl);
        return Ok(new { url = imageUrl });
    }
    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<UserProfile>> GetProfile()
    {
        var profile = await _profileService.GetProfileAsync(GetUserId());

        if (profile == null)
            return NotFound();

        return profile;
    }

    [HttpPost]
    public async Task<ActionResult<UserProfile>> CreateProfile(UserProfile profile)
    {
        var existing = await _profileService.GetProfileAsync(GetUserId());

        if (existing != null)
            return Conflict("Profile already exists");

        var created = await _profileService.CreateProfileAsync(GetUserId(), profile);
        return CreatedAtAction(nameof(GetProfile), created);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UserProfile updated)
    {
        var profile = await _profileService.UpdateProfileAsync(GetUserId(), updated);

        if (profile == null)
            return NotFound();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount()
    {
        await _profileService.DeleteAccountAsync(GetUserId());
        return NoContent();
    }
}