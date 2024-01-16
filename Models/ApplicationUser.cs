using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GreekFreakBackend.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    public string FirstName { get; set; }
    [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
    public string LastName { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    
    public ApplicationUser(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    
    public ApplicationUser()
    {
    }
}