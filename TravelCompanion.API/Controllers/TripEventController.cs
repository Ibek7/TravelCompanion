using Microsoft.AspNetCore.Mvc;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripEventController : ControllerBase
    {
        private readonly TripEventService _tripEventService;
        private readonly TripService _tripService;

        public TripEventController(TripEventService tripEventService, TripService tripService)
        {
            _tripEventService = tripEventService;
            _tripService = tripService;
        }

        [HttpGet("{tripId}")]
        public async Task<ActionResult<IEnumerable<TripEventDto>>> GetAllTripEventsForTrip(int tripId)
        {
            var trip = await _tripService.GetTripDtoByIdAsync(tripId);
            if (trip == null)
            {
                return NotFound($"Trip with ID {tripId} not found");
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to view events for this trip.");
            }

            var tripEvents = await _tripEventService.GetAllTripEventsByTripIdAsync(tripId);
            return Ok(tripEvents);
        }

        [HttpGet("event/{tripEventId}")]
        public async Task<ActionResult<TripEventDto>> GetTripEventById(int tripEventId)
        {
            var tripEvent = await _tripEventService.GetTripEventByIdAsync(tripEventId);
            if (tripEvent == null)
            {
                return NotFound();
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripEvent.TripId);
            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to view this event.");
            }

            return Ok(tripEvent);
        }

        [HttpPost]
        public async Task<ActionResult<TripEventDto>> CreateTripEvent(TripEventDto tripEventDto)
        {
            if (tripEventDto == null)
            {
                return BadRequest("TripEvent data is null");
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripEventDto.TripId);
            if (trip == null)
            {
                return NotFound($"Trip with ID {tripEventDto.TripId} not found");
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to add events to this trip.");
            }

            var createdTripEvent = await _tripEventService.CreateTripEventAsync(tripEventDto);
            return CreatedAtAction(nameof(GetTripEventById), new { tripEventId = createdTripEvent.TripEventId }, createdTripEvent);
        }

        [HttpPut("{tripEventId}")]
        public async Task<ActionResult<TripEventDto>> UpdateTripEvent(int tripEventId, TripEventDto tripEventDto)
        {
            if (tripEventDto == null || tripEventId != tripEventDto.TripEventId)
            {
                return BadRequest("TripEvent data is invalid");
            }

            var tripEvent = await _tripEventService.GetTripEventByIdAsync(tripEventId);
            if (tripEvent == null)
            {
                return NotFound($"TripEvent with ID {tripEventId} not found");
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripEvent.TripId);
            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to update this event.");
            }

            var updatedTripEvent = await _tripEventService.UpdateTripEventAsync(tripEventDto);
            return Ok(updatedTripEvent);
        }

        [HttpDelete("{tripEventId}")]
        public async Task<IActionResult> DeleteTripEvent(int tripEventId)
        {
            var tripEvent = await _tripEventService.GetTripEventByIdAsync(tripEventId);
            if (tripEvent == null)
            {
                return NotFound($"TripEvent with ID {tripEventId} not found");
            }

            var trip = await _tripService.GetTripDtoByIdAsync(tripEvent.TripId);
            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to delete this event.");
            }

            var result = await _tripEventService.DeleteTripEventAsync(tripEventId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
