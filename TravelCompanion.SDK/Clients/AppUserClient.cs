using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TravelCompanion.Domain.DTOs;

namespace TravelCompanion.SDK.Clients
{
    public class AppUserClient
    {
        private readonly HttpClient _httpClient;

        public AppUserClient(HttpClient httpClient, IApiKeyProvider apiKeyProvider)
        {
            _httpClient = httpClient;
            // Add X-Api-Key header to all requests
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKeyProvider.GetApiKey());
        }

        public async Task<AppUserDto> GetUser()
        {
            var response = await _httpClient.GetAsync($"api/appuser");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AppUserDto>();
        }
    }
}
