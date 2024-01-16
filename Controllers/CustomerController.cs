using GreekFreakBackend.Dtos;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreekFreakBackend.Controllers;

[Authorize]
[ApiController]
[Route("my-account")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerController(ICustomerService customerService, UserManager<ApplicationUser> userManager)
    {
        _customerService = customerService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var customerDto = await _customerService.GetCustomerInfoAsync(userId);

        return Ok(customerDto);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateMyAccount([FromBody] CustomerDto customerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized("User is not authenticated.");
        }

        var result = await _customerService.UpdateCustomerInfoAsync(userId, customerDto);
        if (result.Succeeded)
            return Ok();

        return BadRequest(result.Errors);
    }
}