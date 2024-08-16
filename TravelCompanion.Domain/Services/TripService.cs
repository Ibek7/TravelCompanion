using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Models;

namespace TravelCompanion.Domain.Services;

public class TripService
{
    private readonly TravelCompanionContext _context;
    private readonly IMapper _mapper;

    public TripService(TravelCompanionContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TripDto> GetTripDtoByIdAsync(int tripId)
    {
        var trip = await _context.Trips.FindAsync(tripId);
        return _mapper.Map<TripDto>(trip);
    }

    public async Task<IEnumerable<TripDto>> GetAllTripDtosAsync()
    {
        var trips = await _context.Trips.ToListAsync();
        return _mapper.Map<IEnumerable<TripDto>>(trips);
    }

    public async Task<IEnumerable<TripDto>> GetTripsByAppUserIdAsync(int appUserId)
    {
        // Retrieve the trips associated with the given AppUserId
        var trips = await _context.Trips
            .Where(t => t.AppUserId == appUserId)
            .ToListAsync();

        // Map the Trip entities to TripDto objects
        var tripDtos = _mapper.Map<IEnumerable<TripDto>>(trips);

        return tripDtos;
    }

    public async Task<TripDto> CreateTripAsync(TripDto tripDto)
    {
        var trip = _mapper.Map<Trip>(tripDto);
        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();
        return _mapper.Map<TripDto>(trip);
    }

    public async Task<TripDto> UpdateTripAsync(TripDto tripDto)
    {
        // Get trip from _context by user tripDto's TripId
        var trip = await _context.Trips.FindAsync(tripDto.TripId);

        // If the trip doesn't exist, return null
        if (trip == null)
        {
            return null;
        }

        // Map the tripDto to the trip entity
        _mapper.Map(tripDto, trip);

        _context.Trips.Update(trip);
        await _context.SaveChangesAsync();
        return _mapper.Map<TripDto>(trip);
    }

    public async Task<bool> DeleteTripAsync(int tripId)
    {
        // Find the trip by its ID
        var trip = await _context.Trips.FindAsync(tripId);

        // If the trip doesn't exist, return false
        if (trip == null)
        {
            return false;
        }

        // Remove the trip from the context
        _context.Trips.Remove(trip);

        // Save changes to the database
        await _context.SaveChangesAsync();

        // Return true indicating successful deletion
        return true;
    }
}