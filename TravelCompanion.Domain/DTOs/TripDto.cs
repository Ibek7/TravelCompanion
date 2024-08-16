namespace TravelCompanion.Domain.DTOs
{
    public class TripDto
    {
        public int TripId { get; set; }

        public Guid TripGuid { get; set; }

        public int AppUserId { get; set; }

        public string LodgingName { get; set; }

        public string LodgingAddress { get; set; }

        public string LodgingCity { get; set; }

        public string LodgingState { get; set; }

        public string LodgingCountry { get; set; }

        public string LodgingPostalCode { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public DateTime? DepartureDate { get; set; }

        public string TripNotes { get; set; }
    }
}
