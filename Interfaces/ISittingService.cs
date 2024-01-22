using GreekFreakBackend.Dtos;

namespace GreekFreakBackend.Interfaces;

public interface ISittingService
{
    Task<IEnumerable<SittingDto>> GetAvailableSittingsAsync(DateTime reservationTime, int numberOfGuests);
    Task InstantCreateSittingsAsync();
}