using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPanes.Services.Models
{
    /// <summary>
    /// Class represents the full calendar event model.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FullCalendarModel
    {
        [JsonProperty]
        public string id { get; set; }
        [JsonProperty]
        public string title { get; set; }
        [JsonProperty]
        public string start { get; set; }
        [JsonProperty]
        public string end { get; set; }
        [JsonProperty]
        public string room { get; set; }
        [JsonProperty]
        public string owner { get; set; }
        [JsonProperty]
        public string color { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FullCalendarModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the class given individual properties.
        /// </summary>
        public FullCalendarModel(int id, string title, string start, string end, string room, string owner, string color)
        {
            this.id = id.ToString();
            this.title = title;
            this.start = start;
            this.end = end;
            this.room = room;
            this.owner = owner;
            this.color = color;
        }

        /// <summary>
        /// Initializes a new instance of the class and takes in a 
        /// calendar event model.
        /// </summary>
        public FullCalendarModel(CalendarEventModel calendarEvent)
        {
            // convert and set class properties
            id = calendarEvent.EventID.ToString();
            title = calendarEvent.Title;
            start = calendarEvent.StartTime.ToString("yyyy-MM-dd") + "T" + calendarEvent.StartTime.ToString("HH:mm:ss");
            end = calendarEvent.EndTime.ToString("yyyy-MM-dd") + "T" + calendarEvent.EndTime.ToString("HH:mm:ss");
            room = calendarEvent.Room.RoomNumber;
            owner = calendarEvent.EventOwner;
            color = calendarEvent.Cancelled ? @"#ad3a3a" : @"#337ab7";
        }
    }
}
