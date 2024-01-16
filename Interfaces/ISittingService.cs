using GreekFreakBackend.Dtos;

namespace GreekFreakBackend.Interfaces;

public interface ISittingService
{
    Task<IEnumerable<AvailableSittingDto>> GetAvailableSittingsAsync(DateTime reservationTime, int numberOfGuests);
}