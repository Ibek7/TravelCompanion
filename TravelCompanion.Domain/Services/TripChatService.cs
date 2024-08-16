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
    public class TripChatService
    {
        private readonly TravelCompanionContext _context;
        private readonly IMapper _mapper;

        public TripChatService(TravelCompanionContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TripChatInfoDto>> GetAllTripChatInfoByTripIdAsync(int tripId)
        {
            // Retrieve all TripChat entities associated with the given TripId
            var tripChats = await _context.TripChats
                .Where(tc => tc.TripId == tripId)
                .ToListAsync();

            // Map the TripChat entities to TripChatDto objects
            var tripChatDtos = _mapper.Map<IEnumerable<TripChatInfoDto>>(tripChats);

            return tripChatDtos;
        }

        public async Task<TripChatDto> GetByTripChatId(int tripChatId)
        {
            // Retrieve all TripChat entities associated with the given TripId
            var tripChat = await _context.TripChats
                .FirstOrDefaultAsync(tc => tc.TripChatId == tripChatId);

            var tripChatDto = _mapper.Map<TripChatDto>(tripChat);

            return tripChatDto;
        }

        public async Task<TripChatDto> CreateTripChatAsync(TripChatDto tripChatDto)
        {
            // Map the TripChatDto to a TripChat entity
            var tripChat = _mapper.Map<TripChat>(tripChatDto);

            // Add the new TripChat entity to the context
            _context.TripChats.Add(tripChat);

            // Save changes to the database
            await _context.SaveChangesAsync();
            
            // Return the created TripChat as a DTO
            return _mapper.Map<TripChatDto>(tripChat);
        }

        public async Task<TripChatDto> UpdateTripChatAsync(TripChatDto tripChatDto)
        {
            // Find the existing TripChat entity by its TripChatId
            var tripChat = await _context.TripChats.FindAsync(tripChatDto.TripChatId);

            // If the TripChat doesn't exist, return null
            if (tripChat == null)
            {
                return null;
            }

            // Map the updated fields from the DTO to the existing entity
            _mapper.Map(tripChatDto, tripChat);

            // Update the entity in the context
            _context.TripChats.Update(tripChat);

            // Save changes to the database
            await _context.SaveChangesAsync();

            tripChat = await _context.TripChats.FindAsync(tripChat.TripChatId);

            // Return the updated TripChat as a DTO
            return _mapper.Map<TripChatDto>(tripChat);
        }

        public async Task<bool> DeleteTripChatAsync(int tripChatId)
        {
            // Find the TripChat entity by its TripChatId
            var tripChat = await _context.TripChats.FindAsync(tripChatId);

            // If the TripChat doesn't exist, return false
            if (tripChat == null)
            {
                return false;
            }

            // Remove the TripChat entity from the context
            _context.TripChats.Remove(tripChat);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
