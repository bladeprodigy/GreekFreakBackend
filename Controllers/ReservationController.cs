using GreekFreakBackend.Dtos;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreekFreakBackend.Controllers;

[Authorize]
[ApiController]
[Route("reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservationController(IReservationService reservationService, UserManager<ApplicationUser> userManager)
    {
        _reservationService = reservationService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(CreateReservationDto createDto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        try
        {
            var reservationDto = await _reservationService.CreateReservationAsync(userId, createDto);
            return Ok(reservationDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{reservationId:int}")]
    public async Task<IActionResult> GetReservation(int reservationId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var reservation = await _reservationService.GetReservationByIdAsync(reservationId, userId);

        return Ok(reservation);
    }
    
    [HttpGet("my-reservations")]
    public async Task<IActionResult> GetUserReservations()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var reservations = await _reservationService.GetUserReservationsAsync(userId);
        return Ok(reservations);
    }

    [HttpDelete("{reservationId:int}")]
    public async Task<IActionResult> DeleteReservation(int reservationId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var success = await _reservationService.DeleteReservationAsync(reservationId, userId);
        if (!success)
        {
            return NotFound("Reservation not found or not owned by the user.");
        }

        return Ok();
    }
}