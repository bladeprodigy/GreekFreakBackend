using GreekFreakBackend.Dtos;
using GreekFreakBackend.Exceptions;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace GreekFreakBackend.Services;

public class CustomerService : ICustomerService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CustomerDto> GetCustomerInfoAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException($"User with ID {userId} not found.");
        }

        return new CustomerDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task<IdentityResult> UpdateCustomerInfoAsync(string userId, CustomerDto customerDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException($"User with ID {userId} not found.");
        }

        user.FirstName = customerDto.FirstName;
        user.LastName = customerDto.LastName;
        user.Email = customerDto.Email;
        user.PhoneNumber = customerDto.PhoneNumber;

        return await _userManager.UpdateAsync(user);
    }
}