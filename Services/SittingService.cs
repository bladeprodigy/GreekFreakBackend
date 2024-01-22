using GreekFreakBackend.Database;
using GreekFreakBackend.Dtos;
using GreekFreakBackend.Interfaces;
using GreekFreakBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace GreekFreakBackend.Services;

public class SittingService : ISittingService
{
    private readonly ApplicationDbContext _context;

    public SittingService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<SittingDto>> GetAvailableSittingsAsync(DateTime reservationTime, int numberOfGuests)
    {
        var availableSittings = await _context.Sittings
            .Where(s => s.Capacity == numberOfGuests)
            .ToListAsync();

        var availableSittingDtos = new List<SittingDto>();
        foreach (var sitting in availableSittings)
        {
            var isAvailable = await IsSittingAvailableAsync(sitting.SittingId, reservationTime);
            if (isAvailable)
            {
                availableSittingDtos.Add(new SittingDto()
                {
                    SittingId = sitting.SittingId,
                    Capacity = sitting.Capacity,
                    IsOutside = sitting.IsOutside
                });
            }
        }

        return availableSittingDtos;
    }
    
    private async Task<bool> IsSittingAvailableAsync(int sittingId, DateTime reservationTime)
    {
        var overlappingReservations = await _context.Reservations
            .Where(r => r.SittingId == sittingId)
            .Where(r => r.ReservationTime < reservationTime.AddHours(2) &&
                        r.ReservationTime.AddHours(2) > reservationTime)
            .ToListAsync();

        return !overlappingReservations.Any();
    }
    public async Task InstantCreateSittingsAsync()
    {
        if (!_context.Sittings.Any())
        {
            
            for (int i = 1; i <= 10; i++)
            {
                var newSitting = new Sitting
                {
                    
           
                    Capacity = i % 5 + 1,
                    IsOutside = i > 5 
                };

                
                _context.Sittings.Add(newSitting);
            }

            
            await _context.SaveChangesAsync();
        }
    }
}