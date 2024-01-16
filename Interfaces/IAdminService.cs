using GreekFreakBackend.Dtos;
using Microsoft.AspNetCore.Identity;

namespace GreekFreakBackend.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<IdentityResult> DeleteUserAsync(string userId);
    Task InitializeAdminAsync();
    Task<IEnumerable<SittingDto>> GetAllSittingsAsync();
    Task<bool> CreateSittingAsync(AvailableSittingDto sittingDto);
    Task<bool> DeleteSittingAsync(int sittingId);
}