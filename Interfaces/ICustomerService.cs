using GreekFreakBackend.Dtos;
using Microsoft.AspNetCore.Identity;

namespace GreekFreakBackend.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> GetCustomerInfoAsync(string userId);
    Task<IdentityResult> UpdateCustomerInfoAsync(string userId, CustomerDto customerDto);
}