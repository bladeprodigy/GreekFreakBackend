namespace GreekFreakBackend.Dtos;

public class SittingDto
{
    public int SittingId { get; init; }
    public int Capacity { get; set; }
    public bool IsOutside { get; set; }
}