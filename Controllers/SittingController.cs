using GreekFreakBackend.Dtos;
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

    [HttpPost("available")]
    public async Task<IActionResult> GetAvailableSittings(AvailableSittingRequestDto request)
    {
        var availableSittings = await _sittingService.GetAvailableSittingsAsync(request.ReservationTime, request.NumberOfGuests);
        if (!availableSittings.Any())
        {
            return NotFound("There are no available sittings for this amount of people at this time.");
        }

        return Ok(availableSittings);
    }
}