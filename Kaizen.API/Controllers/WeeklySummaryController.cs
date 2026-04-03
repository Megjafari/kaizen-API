using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaizen.API.Services;
using System.Security.Claims;

namespace Kaizen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WeeklySummaryController : ControllerBase
{
    private readonly IWeeklySummaryService _summaryService;

    public WeeklySummaryController(IWeeklySummaryService summaryService)
    {
        _summaryService = summaryService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<WeeklySummary>> GetWeeklySummary([FromQuery] DateTime? weekOf)
    {
        return await _summaryService.GetSummaryAsync(GetUserId(), weekOf);
    }
}