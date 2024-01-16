using GreekFreakBackend.Dtos;
using GreekFreakBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GreekFreakBackend.Controllers;

[ApiController]
public class LoginAndRegisterController : ControllerBase
{
    private readonly ILoginAndRegisterService _loginAndRegisterService;
    
    public LoginAndRegisterController(ILoginAndRegisterService loginAndRegisterService)
    {
        _loginAndRegisterService = loginAndRegisterService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _loginAndRegisterService.RegisterUserAsync(model);
        if (result.Succeeded)
        {
            return Ok("User registered successfully.");
        }

        return BadRequest(result.Errors);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var tokenDto = await _loginAndRegisterService.LoginAsync(model);
            return Ok(tokenDto);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}