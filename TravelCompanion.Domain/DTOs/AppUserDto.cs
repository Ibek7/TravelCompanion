using TravelCompanion.Domain.Models;

namespace TravelCompanion.Domain.DTOs
{
    public class AppUserDto
    {
        public int AppUserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
    }
}
