using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreekFreakBackend.Models;

public class Reservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReservationId { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [Required]
    public int SittingId { get; set; }

    [ForeignKey("SittingId")]
    public virtual Sitting? Sitting { get; set; }

    [Required]
    public DateTime ReservationTime { get; set; }

    [Required]
    public int NumberOfGuests { get; set; }

    public Reservation(string userId, int sittingId, DateTime reservationTime, int numberOfGuests)
    {
        UserId = userId;
        SittingId = sittingId;
        ReservationTime = reservationTime;
        NumberOfGuests = numberOfGuests;
    }
    
    public Reservation()
    {
    }
}