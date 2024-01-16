namespace GreekFreakBackend.Dtos;

public class ReservationDto
{
    public int ReservationId { get; set; }
    public int SittingId { get; set; }
    public DateTime ReservationTime { get; set; }
    public int NumberOfGuests { get; set; }
}