using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreekFreakBackend.Models;

public class Sitting
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SittingId { get; set; }

    [Required]
    public int Capacity { get; set; }

    public bool IsOutside { get; set; }
    
    public virtual ICollection<Reservation>? Reservations { get; set; }
    
    public Sitting(int capacity, bool isOutside)
    {
        Capacity = capacity;
        IsOutside = isOutside;
    }
    
    public Sitting()
    {
    }
}