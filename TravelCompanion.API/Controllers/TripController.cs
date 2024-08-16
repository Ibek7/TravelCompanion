using Microsoft.AspNetCore.Mvc;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripController : ControllerBase
    {
        private readonly TripService _tripService;
        private readonly AppUserService _appUserService;

        public TripController(TripService tripService, AppUserService appUserService)
        {
            _tripService = tripService;
            _appUserService = appUserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetAllTripsForCurrentUser()
        {
            var appUserId = (int)HttpContext.Items["AppUserId"];
            var trips = await _tripService.GetTripsByAppUserIdAsync(appUserId);
            return Ok(trips);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetTripById(int id)
        {
            var trip = await _tripService.GetTripDtoByIdAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (trip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to view information about this trip.");
            }

            return Ok(trip);
        }

        [HttpPost]
        public async Task<ActionResult<TripDto>> CreateTrip(TripDto tripDto)
        {
            if (tripDto == null)
            {
                return BadRequest("Trip data is null");
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            tripDto.AppUserId = appUserId;

            bool userExists = await _appUserService.CheckUserExistsAsync(tripDto.AppUserId);

            if (!userExists)
            {
                return BadRequest($"No user found for App User ID {tripDto.AppUserId}");
            }

            var createdTrip = await _tripService.CreateTripAsync(tripDto);
            return CreatedAtAction(nameof(GetTripById), new { id = createdTrip.TripId }, createdTrip);
        }

        [HttpPut]
        public async Task<ActionResult<TripDto>> UpdateTrip(TripDto tripDto)
        {
            if (tripDto == null)
            {
                return BadRequest("Trip data is invalid");
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (tripDto.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to update information about this trip.");
            }

            bool userExists = await _appUserService.CheckUserExistsAsync(tripDto.AppUserId);

            if (!userExists)
            {
                return BadRequest($"No user found for App User ID {tripDto.AppUserId}");
            }

            // make sure trip exists in the database
            var existingTrip = await _tripService.GetTripDtoByIdAsync(tripDto.TripId);
            if (existingTrip == null)
            {
                return NotFound($"Trip with ID {tripDto.TripId} not found");
            }

            var updatedTrip = await _tripService.UpdateTripAsync(tripDto);
            if (updatedTrip == null)
            {
                return NotFound();
            }

            return Ok(updatedTrip);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            // make sure trip exists in the database
            var existingTrip = await _tripService.GetTripDtoByIdAsync(id);
            if (existingTrip == null)
            {
                return NotFound($"Trip with ID {id} not found");
            }

            var appUserId = (int)HttpContext.Items["AppUserId"];
            if (existingTrip.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed to delete this trip.");
            }

            var result = await _tripService.DeleteTripAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
