using GreekFreakBackend.Database;
using GreekFreakBackend.Dtos;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace GreekFreakBackend.Services;

public class ReservationService : IReservationService
{
    private readonly ApplicationDbContext _context;
    
    public ReservationService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ReservationDto> CreateReservationAsync(string userId, CreateReservationDto createDto)
    {

        var reservation = new Reservation
        {
            UserId = userId,
            SittingId = createDto.SelectedSittingId,
            ReservationTime = createDto.ReservationTime,
            NumberOfGuests = createDto.NumberOfGuests
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return new ReservationDto
        {
            ReservationId = reservation.ReservationId,
            SittingId = reservation.SittingId,
            ReservationTime = reservation.ReservationTime,
            NumberOfGuests = reservation.NumberOfGuests
        };
    }
    
    public async Task<ReservationDto> GetReservationByIdAsync(int reservationId, string userId)
    {
        var reservation = await _context.Reservations
            .Where(r => r.ReservationId == reservationId && r.UserId == userId)
            .Select(r => new ReservationDto
            {
                ReservationId = r.ReservationId,
                SittingId = r.SittingId,
                ReservationTime = r.ReservationTime,
                NumberOfGuests = r.NumberOfGuests
            })
            .FirstOrDefaultAsync();

        return reservation;
    }
    
    public async Task<IEnumerable<ReservationDto>> GetUserReservationsAsync(string userId)
    {
        return await _context.Reservations
            .Where(r => r.UserId == userId)
            .Select(r => new ReservationDto
            {
                ReservationId = r.ReservationId,
                SittingId = r.SittingId,
                ReservationTime = r.ReservationTime,
                NumberOfGuests = r.NumberOfGuests
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteReservationAsync(int reservationId, string userId)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.UserId == userId);

        if (reservation == null)
        {
            return false;
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }
}