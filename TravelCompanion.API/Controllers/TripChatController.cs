using Microsoft.AspNetCore.Mvc;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripChatController : ControllerBase
    {
        private readonly TripChatService _tripChatService;
        private readonly TripService _tripService;

        public TripChatController(TripChatService tripChatService, TripService tripService)
        {
            _tripChatService = tripChatService;
            _tripService = tripService;
        }

        [HttpGet("{tripId}")]
        public async Task<ActionResult<IEnumerable<TripChatInfoDto>>> GetAllTripChatsForTrip(int tripId)
        {
            var trip = await _tripService.GetTripDtoByIdAsync(tripId);
            if (trip == null)
            {
                return NotFound($"Trip with ID {tripId} not found");
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to view chats for this trip.");
            }

            var tripChats = await _tripChatService.GetAllTripChatInfoByTripIdAsync(tripId);
            return Ok(tripChats);
        }

        [HttpGet("chat/{tripChatId}")]
        public async Task<ActionResult<TripChatDto>> GetTripChatById(int tripChatId)
        {
            var tripChat = await _tripChatService.GetByTripChatId(tripChatId);
            if (tripChat == null)
            {
                return NotFound();
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripChat.TripId);
            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to view this chat.");
            }

            return Ok(tripChat);
        }

        [HttpPost]
        public async Task<ActionResult<TripChatDto>> CreateTripChat(TripChatDto tripChatDto)
        {
            if (tripChatDto == null)
            {
                return BadRequest("TripChat data is null");
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripChatDto.TripId);
            if (trip == null)
            {
                return NotFound($"Trip with ID {tripChatDto.TripId} not found");
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to add chats to this trip.");
            }

            var createdTripChat = await _tripChatService.CreateTripChatAsync(tripChatDto);
            return CreatedAtAction(nameof(GetTripChatById), new { tripChatId = createdTripChat.TripChatId }, createdTripChat);
        }

        [HttpPut("{tripChatId}")]
        public async Task<ActionResult<TripChatDto>> UpdateTripChat(int tripChatId, TripChatDto tripChatDto)
        {
            if (tripChatDto == null || tripChatId != tripChatDto.TripChatId)
            {
                return BadRequest("TripChat data is invalid");
            }

            var tripChat = await _tripChatService.GetByTripChatId(tripChatId);
            if (tripChat == null)
            {
                return NotFound($"TripChat with ID {tripChatId} not found");
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripChat.TripId);
            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to update this chat.");
            }

            var updatedTripChat = await _tripChatService.UpdateTripChatAsync(tripChatDto);
            return Ok(updatedTripChat);
        }

        [HttpDelete("{tripChatId}")]
        public async Task<IActionResult> DeleteTripChat(int tripChatId)
        {
            var tripChat = await _tripChatService.GetByTripChatId(tripChatId);
            if (tripChat == null)
            {
                return NotFound($"TripChat with ID {tripChatId} not found");
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripChat.TripId);
            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to delete this chat.");
            }

            var result = await _tripChatService.DeleteTripChatAsync(tripChatId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
