using DoorPanes.Services.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoorPanes.Services.DAL
{
    public class CalendarEventRepository : ICalendarEventRepository, IDisposable
    {
        // private instance of the database context
        private ApplicationDbContext context;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="context">Insance of the db context</param>
        public CalendarEventRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Returns a list of all calendar events.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CalendarEventModel> GetCalendarEvents()
        {
            return context.CalendarEvents.Include(x => x.Room).ToList();
        }

        /// <summary>
        /// Inserts a calendar event into the database.
        /// </summary>
        /// <param name="calendarEvent"></param>
        public void InsertCalendarEvent(CalendarEventModel calendarEvent)
        {
            context.Entry(calendarEvent.Room).State = EntityState.Unchanged;
            context.CalendarEvents.Add(calendarEvent);
        }

        /// <summary>
        /// Updates a calendar event model.
        /// </summary>
        /// <param name="calendarEvent">The new room data to update.</param>
        public void UpdateCalendarEvent(CalendarEventModel calendarEvent)
        {
            if (calendarEvent == null)
                return;

            // remove old event
            var badEvent = context.CalendarEvents.Where(e => e.EventID == calendarEvent.EventID).FirstOrDefault();
            context.CalendarEvents.Remove(badEvent);

            // add new event
            context.Entry(calendarEvent.Room).State = EntityState.Unchanged;
            context.CalendarEvents.Add(calendarEvent);
        }

        /// <summary>
        /// Removes a caledar event from the database.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCalendarEvent(long id)
        {
            var result = context.CalendarEvents.Where(m => m.EventID == id).ToList();
            if (result.Count == 1)
                context.CalendarEvents.Remove(result.ElementAt(0));
        }

        /// <summary>
        /// Removes all calendar events from the database.
        /// </summary>
        public void DeleteAllCalendarEvents()
        {
            foreach (var entry in context.CalendarEvents.ToList())
            {
                context.CalendarEvents.Remove(entry);
            }
         }

        /// <summary>
        /// Method to find a calendar event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Find(int id)
        {
            var calEvent = context.CalendarEvents.Where(e => e.EventID == id).FirstOrDefault();
            return calEvent == null ? false : true;
        }

        /// <summary>
        /// Saves all changes made to the calendar event database context.
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}