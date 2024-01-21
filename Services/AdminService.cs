using GreekFreakBackend.Database;
using GreekFreakBackend.Dtos;
using GreekFreakBackend.Exceptions;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GreekFreakBackend.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = userRoles.ToList()
            });
        }

        return userDtos;
    }

    public async Task<IdentityResult> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException($"User with ID {userId} not found.");
        }

        return await _userManager.DeleteAsync(user);
    }

    public async Task InitializeAdminAsync()
    {
        const string adminEmail = "admin@example.com";
        var adminUser = await _userManager.FindByEmailAsync(adminEmail);

        if (adminUser != null)
        {
            return;
        }

        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Zoro",
            LastName = "Roronoa",
            PhoneNumber = "333333333"
        };

        var createUserResult = await _userManager.CreateAsync(adminUser, "BetterThanSanji123");
        if (!createUserResult.Succeeded)
        {
            return;
        }

        await _userManager.AddToRoleAsync(adminUser, "Admin");
        await _userManager.AddToRoleAsync(adminUser, "Customer");
    }

    public async Task<IEnumerable<SittingDto>> GetAllSittingsAsync()
    {
        return await _context.Sittings
            .Select(s => new SittingDto
            {
                SittingId = s.SittingId,
                Capacity = s.Capacity,
                IsOutside = s.IsOutside
            })
            .ToListAsync();
    }

    public async Task<bool> CreateSittingAsync(AvailableSittingDto sittingDto)
    {
        var sitting = new Sitting
        {
            Capacity = sittingDto.Capacity,
            IsOutside = sittingDto.IsOutside
        };

        _context.Sittings.Add(sitting);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSittingAsync(int sittingId)
    {
        var sitting = await _context.Sittings.FindAsync(sittingId);
        if (sitting == null)
        {
            return false;
        }

        _context.Sittings.Remove(sitting);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
    {
        return await _context.Reservations
            .Select(r => new ReservationDto
            {
                ReservationId = r.ReservationId,
                SittingId = r.SittingId,
                ReservationTime = r.ReservationTime,
                NumberOfGuests = r.NumberOfGuests
            }
            )
            .ToListAsync();
    }
    public async Task<bool> DeleteReservationAsync(int reservationId)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId);

        if (reservation == null)
        {
            return false; // Reservation not found 
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }
}