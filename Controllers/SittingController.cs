using GreekFreakBackend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreekFreakBackend.Controllers;

[Authorize]
[ApiController]
[Route("sittings")]
public class SittingController : ControllerBase
{
    private readonly ISittingService _sittingService;

    public SittingController(ISittingService sittingService)
    {
        _sittingService = sittingService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableSittings(DateTime reservationTime, int numberOfGuests)
    {
        var availableSittings = await _sittingService.GetAvailableSittingsAsync(reservationTime, numberOfGuests);
        if (!availableSittings.Any())
        {
            return NotFound("There are no available sittings for this amount of people at this time.");
        }

        return Ok(availableSittings);
    }
}