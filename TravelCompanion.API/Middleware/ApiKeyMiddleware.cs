using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TravelCompanion.API.Models;
using TravelCompanion.Domain.Models;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.API.Middleware
{
    public class ApiKeyMiddleware
    {
        public const string API_KEY_HEADER = "X-API-Key";

        private RequestDelegate Next { get; }
        public ApiKeyMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMemoryCache memoryCache, AppUserService appUserService)
        {
            if (context.Request.Path.ToString().StartsWith("/rin")
                || context.Request.Path.ToString().StartsWith("/mobileauth")
                || context.Request.Path.ToString().Equals("/favicon.ico"))
            {
                await Next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("X-Error-Msg", "MISSING_API_KEY");
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new ErrorResponse
                        {
                            Message = "API Key was not provided."
                        }));
                return;
            }

            if (memoryCache.TryGetValue(extractedApiKey.First(), out int appUserId))
            {
                context.Items["AppUserId"] = appUserId;
                context.Items["AppUserGuid"] = extractedApiKey;

                await Next(context);
                return;
            }

            if (extractedApiKey.Distinct().Count() > 1)
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("X-Error-Msg", "API_KEY_INVALID_HEADERS");
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new ErrorResponse
                        {
                            Message = "More than one API Key was specified."
                        }));
                return;
            }

            if (!Guid.TryParse(extractedApiKey.First(), out var guidResult))
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("X-Error-Msg", "API_KEY_INVALID_FORMAT");
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new ErrorResponse
                        {
                            Message = "API Key was not in the correct format."
                        }));
                return;
            }

            var user = await appUserService.GetByGuidAsync(guidResult);
            if (user == null)
            {
                context.Response.StatusCode = 401;
                context.Response.Headers.Append("X-Error-Msg", "API_KEY_INVALID");
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        new ErrorResponse
                        {
                            Message = "API Key was not valid."
                        }));
                return;
            }

            memoryCache.Set(extractedApiKey.First(), user.AppUserId, TimeSpan.FromDays(1));

            context.Items["AppUserId"] = user.AppUserId;
            context.Items["AppUserGuid"] = guidResult;

            await Next(context);
        }
    }
}
