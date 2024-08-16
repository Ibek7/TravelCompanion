using Microsoft.AspNetCore.Mvc;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserController : ControllerBase
    {
        private readonly AppUserService _appUserService;

        public AppUserController(AppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public async Task<ActionResult<AppUserDto>> GetUser()
        {
            var appUserId = (int)HttpContext.Items["AppUserId"];
            var user = await _appUserService.GetByIdAsync(appUserId);
            if (user == null)
            {
                return NotFound();
            }

            if (user.AppUserId != appUserId)
            {
                return Unauthorized("Not allowed.");
            }
            return Ok(user);
        }
    }
}
