using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kaizen.API.Data;
using Kaizen.API.Models;
using System.Security.Claims;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly KaizenDbContext _context;

    public ProfileController(KaizenDbContext context)
    {
        _context = context;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<UserProfile>> GetProfile()
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == GetUserId());

        if (profile == null)
            return NotFound();

        return profile;
    }

    [HttpPost]
    public async Task<ActionResult<UserProfile>> CreateProfile(UserProfile profile)
    {
        profile.UserId = GetUserId();

        var existing = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == profile.UserId);

        if (existing != null)
            return Conflict("Profile already exists");

        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProfile), profile);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UserProfile updated)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == GetUserId());

        if (profile == null)
            return NotFound();

        profile.Height = updated.Height;
        profile.Weight = updated.Weight;
        profile.Age = updated.Age;
        profile.Gender = updated.Gender;
        profile.Goal = updated.Goal;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}