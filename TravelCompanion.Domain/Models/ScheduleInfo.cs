using System;

namespace TravelCompanion.Domain.Models
{
    public class ScheduleInfo
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
    }
}
