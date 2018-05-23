using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DoorPanes.Services.Models
{
    public class CalendarEventModel
    {
        [Key]
        public int EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long RepeatID { get; set; }
        public string EventOwner { get; set; }
        public RoomModel Room { get; set; }
        public bool Cancelled { get; set; }
        public EventType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CalendarEventModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the class given a full calendar model.
        /// </summary>
        public CalendarEventModel(FullCalendarModel fullCalModel)
        {
            // set class properties
            EventID = Convert.ToInt32(fullCalModel.id);
            Title = fullCalModel.title;
            Description = "Event description goes here...";
            StartTime = ConvertToDateTime(fullCalModel.start);
            EndTime = ConvertToDateTime(fullCalModel.end);
            Room = GetRoomModelFromNumber(fullCalModel.room);
            EventOwner = fullCalModel.owner;
            Cancelled = fullCalModel.color == @"#337ab7" ? false : true;
        }

        /// <summary>
        /// Converts the full calendar datetime string to C# datetime object.
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        private DateTime ConvertToDateTime(string timeString)
        {
            // time specifics
            string dateAndTimeFormat = "yyyy-MM-dd HH:mm:ss";
            CultureInfo provider = CultureInfo.InvariantCulture;
            return DateTime.ParseExact(timeString.Replace('T', ' '), dateAndTimeFormat, provider);
        }

        /// <summary>
        /// Returns a room model given a room number.
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        private RoomModel GetRoomModelFromNumber(string roomNumber)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Rooms.Where(e => e.RoomNumber == roomNumber).FirstOrDefault();
        }
    }

    public enum EventType
    {
        Class,
        Meeting,
        OfficeHours
    }
}