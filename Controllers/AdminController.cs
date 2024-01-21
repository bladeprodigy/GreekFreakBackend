using GreekFreakBackend.Dtos;
using GreekFreakBackend.Exceptions;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using GreekFreakBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreekFreakBackend.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    
    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var userDtos = await _adminService.GetAllUsersAsync();
        return Ok(userDtos);
    }
    
    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            var result = await _adminService.DeleteUserAsync(userId);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("sittings")]
    public async Task<IActionResult> GetAllSittings()
    {
        var sittings = await _adminService.GetAllSittingsAsync();
        return Ok(sittings);
    }
    
    [HttpPost("sittings")]
    public async Task<IActionResult> CreateSitting(AvailableSittingDto sittingDto)
    {
        var success = await _adminService.CreateSittingAsync(sittingDto);
        if (!success)
        {
            return BadRequest("Unable to create the sitting.");
        }

        return Ok();
    }
    
    [HttpDelete("sittings/{sittingId:int}")]
    public async Task<IActionResult> DeleteSitting(int sittingId)
    {
        var success = await _adminService.DeleteSittingAsync(sittingId);
        if (!success)
        {
            return NotFound("Sitting not found.");
        }

        return Ok();
    }

    [HttpGet("reservations")]
    public async Task<IActionResult> GetAllReservations()
    {
        var reservations = await _adminService.GetAllReservationsAsync();
        return Ok(reservations);
    }

    [HttpDelete("{reservationId:int}")]
    public async Task<IActionResult> DeleteReservation(int reservationId)
    {
        var success = await _adminService.DeleteReservationAsync(reservationId);
        if (!success)
        {
            return NotFound("Reservation not found.");
        }

        return Ok();
    }
}