using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Models;

namespace TravelCompanion.Domain.Services
{
    public class TripEventService
    {
        private readonly TravelCompanionContext _context;
        private readonly IMapper _mapper;

        public TripEventService(TravelCompanionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TripEventDto>> GetAllTripEventsByAppUserIdAsync(int appUserId)
        {
            // Retrieve all Trip entities associated with the given AppUserId
            var trips = await _context.Trips
                .Where(t => t.AppUserId == appUserId)
                .ToListAsync();

            // Retrieve all TripEvent entities associated with the given TripIds
            var tripEvents = await _context.TripEvents
                .Where(te => trips.Select(t => t.TripId).Contains(te.TripId))
                .ToListAsync();

            // Map the TripEvent entities to TripEventDto objects
            var tripEventDtos = _mapper.Map<IEnumerable<TripEventDto>>(tripEvents);

            return tripEventDtos;
        }

        public async Task<IEnumerable<TripEventDto>> GetAllTripEventsByTripIdAsync(int tripId)
        {
            // Retrieve all TripEvent entities associated with the given TripId
            var tripEvents = await _context.TripEvents
                .Where(te => te.TripId == tripId)
                .ToListAsync();

            // Map the TripEvent entities to TripEventDto objects
            var tripEventDtos = _mapper.Map<IEnumerable<TripEventDto>>(tripEvents);

            return tripEventDtos;
        }

        public async Task<TripEventDto> GetTripEventByIdAsync(int tripEventId)
        {
            // Find the TripEvent entity by its TripEventId
            var tripEvent = await _context.TripEvents
                .FirstOrDefaultAsync(te => te.TripEventId == tripEventId);

            // If the TripEvent doesn't exist, return null
            if (tripEvent == null)
            {
                return null;
            }

            // Map the TripEvent entity to a TripEventDto object
            return _mapper.Map<TripEventDto>(tripEvent);
        }

        public async Task<TripEventDto> CreateTripEventAsync(TripEventDto tripEventDto)
        {
            // Map the TripEventDto to a TripEvent entity
            var tripEvent = _mapper.Map<TripEvent>(tripEventDto);

            // Add the new TripEvent entity to the context
            _context.TripEvents.Add(tripEvent);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created TripEvent as a DTO
            return _mapper.Map<TripEventDto>(tripEvent);
        }

        public async Task<TripEventDto> UpdateTripEventAsync(TripEventDto tripEventDto)
        {
            // Find the existing TripEvent entity by its TripEventId
            var tripEvent = await _context.TripEvents.FindAsync(tripEventDto.TripEventId);

            // If the TripEvent doesn't exist, return null
            if (tripEvent == null)
            {
                return null;
            }

            // Map the updated fields from the DTO to the existing entity
            _mapper.Map(tripEventDto, tripEvent);

            // Update the entity in the context
            _context.TripEvents.Update(tripEvent);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the updated TripEvent as a DTO
            return _mapper.Map<TripEventDto>(tripEvent);
        }

        public async Task<bool> DeleteTripEventAsync(int tripEventId)
        {
            // Find the TripEvent entity by its TripEventId
            var tripEvent = await _context.TripEvents.FindAsync(tripEventId);

            // If the TripEvent doesn't exist, return false
            if (tripEvent == null)
            {
                return false;
            }

            // Remove the TripEvent entity from the context
            _context.TripEvents.Remove(tripEvent);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
