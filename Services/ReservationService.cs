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
        var existingReservation = await _context.Reservations
            .AnyAsync(r => r.UserId == userId && 
                           r.ReservationTime.Date == createDto.ReservationTime.Date);

        if (existingReservation)
        {
            throw new InvalidOperationException("User can only make one reservation per day.");
        }
        
        if (createDto.ReservationTime.Hour < 14 || createDto.ReservationTime.Hour >= 24)
        {
            throw new InvalidOperationException("Reservations can only be made from 2pm to 12am.");
        }

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
                // Map other properties as needed
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
                // Map other properties as needed
            })
            .ToListAsync();
    }

    public async Task<bool> DeleteReservationAsync(int reservationId, string userId)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.ReservationId == reservationId && r.UserId == userId);

        if (reservation == null)
        {
            return false; // Reservation not found or not owned by the user
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }
}