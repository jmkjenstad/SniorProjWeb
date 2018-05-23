using DoorPanes.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorPanes.Services.DAL
{
    public interface ICalendarEventRepository : IDisposable
    {
        IEnumerable<CalendarEventModel> GetCalendarEvents();
        //CalendarEvent GetCalendarEventsByDate(DateTime startDate, DateTime endDate);
        //IEnumerable<CalendarEvent> GetCalendarEvents(string Owner);
        void InsertCalendarEvent(CalendarEventModel calendarEvent);

        void UpdateCalendarEvent(CalendarEventModel calendarEvent);
        void DeleteCalendarEvent(long id);
        void DeleteAllCalendarEvents();
        //void UpdateCalendarEvent(CalendarEvent student);
        bool Find(int id);
        void Save();
    }
}
