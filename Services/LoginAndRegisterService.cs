using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GreekFreakBackend.Dtos;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GreekFreakBackend.Services;

public class LoginAndRegisterService : ILoginAndRegisterService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    
    public LoginAndRegisterService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task CreateRoles()
    {
        string[] roleNames = { "Admin", "Customer" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public async Task<IdentityResult> RegisterUserAsync(RegisterDto model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email) != null;
        if (userExists)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User with this email already exists." });
        }

        var phoneExists = await _userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber);
        if (phoneExists)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User with this phone number already exists." });
        }

        var user = new ApplicationUser(model.FirstName, model.LastName)
        {
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return result;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
        return !roleResult.Succeeded ? 
            IdentityResult.Failed(new IdentityError { Description = "Failed to assign the user role." }) : IdentityResult.Success;
    }
    
    public async Task<TokenDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            throw new UnauthorizedAccessException("Login failed: Invalid email or password.");

        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        var key = Encoding.ASCII.GetBytes(jwtKey);

        if (!double.TryParse(_configuration["Jwt:ExpiresInMinutes"], out var expiresInMinutes))
        {
            throw new InvalidOperationException("Invalid JWT expiration configuration.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            }),
            Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new TokenDto
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = token.ValidTo
        };
    }


}