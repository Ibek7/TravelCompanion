using System;
using System.Collections.ObjectModel;

namespace TravelCompanion.Domain.Models
{
    public class ScheduleRepository
    {
        public ScheduleRepository() { }

        public ObservableCollection<ScheduleInfo> GetScheduleInfos()
        {
            var listInfo = new ObservableCollection<ScheduleInfo>();
            var eventDate = DateTime.Now.AddDays(3);
            var events = new[]
            {
                new
                {
                    from = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, 8, 0, 0),
                    to = new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, 10, 0, 0),
                    eventname = "First Trip",
                    eventdescription = "will visit Ifle Tower" 
                }
            };

            for (int i = 0; i < events.Length; i++)
            {
                var info = new ScheduleInfo
                {
                    From = events[i].from,
                    To = events[i].to,
                    EventName = events[i].eventname,
                    EventDescription = events[i].eventdescription,
                };
                listInfo.Add(info);
            }

            return listInfo;
        }
    }
}
