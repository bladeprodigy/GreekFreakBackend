using GreekFreakBackend.Dtos;
using Microsoft.AspNetCore.Identity;

namespace GreekFreakBackend.Interfaces;

public interface ILoginAndRegisterService
{
    Task CreateRoles();
    Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
    Task<TokenDto> LoginAsync(LoginDto loginDto);
}