using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.API.Controllers
{
	[Route("mobileauth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		const string callbackScheme = "myapp";
		private readonly AppUserService _appUserService;

		public AuthController(AppUserService appUserService)
        {
			_appUserService = appUserService;
        }

		[HttpGet("{scheme}")]
		public async Task Get([FromRoute] string scheme)
		{
			var auth = await Request.HttpContext.AuthenticateAsync(scheme);

			if (!auth.Succeeded
				|| auth?.Principal == null
				|| !auth.Principal.Identities.Any(id => id.IsAuthenticated)
				|| string.IsNullOrEmpty(auth.Properties.GetTokenValue("access_token")))
			{
				// Not authenticated, challenge
				await Request.HttpContext.ChallengeAsync(scheme);
			}
			else
			{
				var claims = auth.Principal.Identities.FirstOrDefault()?.Claims;
				var email = string.Empty;
				email = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

                var user = await _appUserService.GetByEmailAsync(email);
				if (user == null)
                {
                    // Create user
                    var newUser = new AppUserDto
                    {
                        Email = email,
                        FirstName = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.GivenName)?.Value,
                        LastName = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Surname)?.Value
                    };
                    user = await _appUserService.CreateUserAsync(newUser);
                }
				var guid = await _appUserService.GetAppUserGuid(user.AppUserId);

                // Get parameters to send back to the callback
                var qs = new Dictionary<string, string>
				{
                    { "api_key", guid.ToString() },
                    { "access_token", auth.Properties.GetTokenValue("access_token") },
					{ "refresh_token", auth.Properties.GetTokenValue("refresh_token") ?? string.Empty },
					{ "expires_in", (auth.Properties.ExpiresUtc?.ToUnixTimeSeconds() ?? -1).ToString() },
					{ "email", email }
				};

				// Build the result url
				var url = callbackScheme + "://#" + string.Join(
					"&",
					qs.Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Value != "-1")
					.Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

				// Redirect to final url
				Request.HttpContext.Response.Redirect(url);
			}
		}
	}
}
