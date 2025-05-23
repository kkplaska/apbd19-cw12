using apbd19_cw12.Data;
using apbd19_cw12.DTOs;
using apbd19_cw12.Exceptions;
using apbd19_cw12.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd19_cw12.Services;

public class DbService : IDbService
{
    private readonly ApbdContext _context;
    public DbService(ApbdContext context)
    {
        _context = context;
    }

    public async Task<TripsPageDto> GetTripsAsync(int pageNum = 1, int pageSize = 10)
    {
        if (pageNum < 1) pageNum = 1;
        if (pageSize < 1) pageSize = 10;
        var allTrips = await _context.Trips.CountAsync();
        var allPages = (int) Math.Ceiling(allTrips / (double) pageSize);

        var trips = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(trip => new TripDto()
        {
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            MaxPeople = trip.MaxPeople,
            Countries = trip.IdCountries.Select(c => new CountryDto()
            {
                Name = c.Name,
            }).OrderBy(c => c.Name).ToList(),
            Clients = trip.ClientTrips.Select(a => a.IdClientNavigation).Select(c => new ClientDto()
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
            }).ToList()
        }).ToListAsync();
        
        return new TripsPageDto()
        {
            PageNum = pageNum,
            PageSize = pageSize,
            AllPages = allPages, 
            Trips = trips
        };
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            throw new KeyNotFoundException();
        }
        if (_context.ClientTrips.Any(ct => ct.IdClient == client.IdClient))
        {
            throw new AssignedTripsException();
        }
        
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}