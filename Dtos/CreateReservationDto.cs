namespace GreekFreakBackend.Dtos;

public class CreateReservationDto
{
    public int SelectedSittingId { get; set; }
    public DateTime ReservationTime { get; set; }
    public int NumberOfGuests { get; set; }
}