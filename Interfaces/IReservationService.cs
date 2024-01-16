using GreekFreakBackend.Dtos;

namespace GreekFreakBackend.Interfaces;

public interface IReservationService
{
    Task<ReservationDto> CreateReservationAsync(string userId, CreateReservationDto createDto);
    Task<ReservationDto> GetReservationByIdAsync(int reservationId, string userId);
    Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(string userId);
    Task<bool> DeleteReservationAsync(int reservationId, string userId);
}