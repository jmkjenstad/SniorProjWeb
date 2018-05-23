using DoorPanes.Services.DAL;
using DoorPanes.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace DoorPanes.WebApi.Controllers
{
    /// <summary>
    /// Calendar event controller class.
    /// Author: Samantha Kranstz, Andrew Fagrey
    /// </summary>
    [Authorize]
    //[RoutePrefix("api/Calendar")]
    public class CalendarController : ApiController
    {
        // private instance of cal event repo
        private ICalendarEventRepository calendarEventRepository;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CalendarController()
        {
            this.calendarEventRepository = new CalendarEventRepository(new ApplicationDbContext());
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="calendarEventRepository">A Calendar Event Repository Instance</param>
        public CalendarController(ICalendarEventRepository calendarEventRepository)
        {
            this.calendarEventRepository = calendarEventRepository;
        }

        /// <summary>
        /// Returns a full list of all calendar events.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetCalendarEvents()
        {
            // get a list of calendar events
            var calendarEvents = calendarEventRepository.GetCalendarEvents().ToList();

            HttpResponseMessage response;

            // return 404 if there are no calendar events found
            if (calendarEvents == null || calendarEvents.Count == 0)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("No calendar entries found!", Encoding.UTF8);
                return response;
            }

            // return the calendar event entries with a status code of OK
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(calendarEvents), 
                Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Returns a list of calendar events based on owner.
        /// </summary>
        /// <param name="Owner">The calendar event owner.</param>
        /// <returns></returns>
        public HttpResponseMessage GetCalendarEventsByOwner(string Owner)
        {
            HttpResponseMessage response;

            // Get all events from database
            var calendarEvents = from e in calendarEventRepository.GetCalendarEvents() select e;

            // Check if the owner string provided is valid
            if (!string.IsNullOrEmpty(Owner))
            {
                // Populate the list with events only for owner provided
                calendarEvents = calendarEvents.Where(e => e.EventOwner.ToUpper().Contains(Owner.ToUpper()));

                // Check to see if there are any events for the particular owner
                if (calendarEvents.Count() != 0)
                {
                    // Set response to the list serialized into JSON
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(JsonConvert.SerializeObject(calendarEvents),
                        Encoding.UTF8, "application/json");
                }
                // Otherwise the database is empty or the owner has no events in the database
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent("No calendar entries found!", Encoding.UTF8);
                    return response;
                }
            }
            // Return a response indicating that the user entered an invalid owner
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Invalid owner.");
            }

            return response;
        }

        /// <summary>
        /// Returns a list of calendar events based on a room.
        /// </summary>
        /// <param name="room">The room that hosts the events.</param>
        /// <returns></returns>
        public HttpResponseMessage GetCalendarEventsByRoom(string room)
        {
            HttpResponseMessage response;
            
            // check room number parameter
            if (string.IsNullOrEmpty(room))
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Invalid room given");
                return response;
            }

            // select the calendar events from the database that are for the given room
            var eventList = calendarEventRepository.GetCalendarEvents().Where(e => e.Room.RoomNumber == room);

            // check for events
            if (eventList.Count() == 0)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("No calendar entries found for given room!", Encoding.UTF8);
                return response;
            }

            // otherwise return list of events that are for the given room
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(eventList),
                Encoding.UTF8, "application/json");
            return response;
        }

        /// <summary>
        /// Returns a list of calendar events based on a owner and semester.
        /// </summary>
        /// <param name="Semester">The semester that contains the events.</param>
        /// <param name="Owner">The calendar events' owner.</param>
        /// <returns></returns>
        public HttpResponseMessage GetCalendarEventsByRange_Owner(string Semester, string Owner)
        {
            HttpResponseMessage response;

            // Get all events from database
            var calendarEvents = from e in calendarEventRepository.GetCalendarEvents() select e;

            // Check if the owner and semester strings provided are valid
            if (!string.IsNullOrEmpty(Owner) && !string.IsNullOrEmpty(Semester))
            {
                // Populate the list with events only for owner provided
                calendarEvents = calendarEvents.Where(e => e.EventOwner.ToUpper().Contains(Owner.ToUpper()));

                // Check to see if the database is empty or the owner has no events in the database
                if (calendarEvents.Count() == 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent("No calendar entries found that owner!", Encoding.UTF8);
                    return response;
                }
            }
            // Return a response indicating that the user entered an invalid owner
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Invalid owner or semester.");
                return response;
            }

            // Create a new list to add the events that in both the room and semester
            List<CalendarEventModel> rangeList = new List<CalendarEventModel>();
            List<CalendarEventModel> ownerList = calendarEvents.ToList();
            string semester = Semester.ToLower();

            // Fall Semester
            if (semester == "fall")
            {
                DateTime startDay = new DateTime(DateTime.Now.Year, 8, 1);
                DateTime endDay = new DateTime(DateTime.Now.Year, 12, 31);

                // find events between august and december
                foreach( CalendarEventModel ce in ownerList )
                {
                    if( ce.StartTime.Date > startDay.Date && ce.StartTime.Date < endDay.Date )
                    {
                        rangeList.Add( ce );
                    }
                }

            }

            // Spring Semester
            else if (semester == "spring")
            {
                DateTime startDay = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime endDay = new DateTime(DateTime.Now.Year, 5, 31);

                // find events between january and may
                foreach (CalendarEventModel ce in ownerList)
                {
                    if (ce.StartTime.Date > startDay.Date && ce.StartTime.Date < endDay.Date)
                    {
                        rangeList.Add(ce);
                    }
                }
            }

            // Summer Semester
            else if (semester == "summer")
            {
                DateTime startDay = new DateTime(DateTime.Now.Year, 5, 1);
                DateTime endDay = new DateTime(DateTime.Now.Year, 8, 31);

                // find events between may and august
                foreach (CalendarEventModel ce in ownerList)
                {
                    if (ce.StartTime.Date > startDay.Date && ce.StartTime.Date < endDay.Date)
                    {
                        rangeList.Add(ce);
                    }
                }
            }

            // Invalid Semester
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Not a recognized semester.");
                return response;
            }

            // Check to see if there are any events for the particular owner and semester
            if (rangeList.Count() != 0)
            {
                // Set response to the list serialized into JSON
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(rangeList),
                    Encoding.UTF8, "application/json");
            }
            // Otherwise the database is empty or the owner has no events in the database
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("No calendar entries found for that owner in that semester!", Encoding.UTF8);
                return response;
            }

            return response;

        }

        /// <summary>
        /// Returns a list of calendar events based on a room and semester.
        /// </summary>
        /// <param name="Semester">The semester that contains the events.</param>
        /// <param name="Room">The room that hosts the calendar events.</param>
        /// <returns></returns>
        public HttpResponseMessage GetCalendarEventsByRange_Room(string Semester, string Room)
        {
            HttpResponseMessage response;

            // Get all events from database
            var calendarEvents = from e in calendarEventRepository.GetCalendarEvents() select e;
            List<CalendarEventModel> calendarEventList = calendarEvents.ToList();
            List<CalendarEventModel> roomList = new List<CalendarEventModel>();


            // Check if the room and semester strings provided are valid
            if (!string.IsNullOrEmpty(Room) && !string.IsNullOrEmpty(Semester))
            {
                foreach( CalendarEventModel ce in calendarEventList )
                {

                    // Add the events for the given room to a list
                    if (ce.Room != null)
                    {
                        if (ce.Room.RoomNumber.ToUpper().Contains(Room.ToUpper()))
                        {
                            roomList.Add(ce);
                        }
                    }
                }

                // Check to see if the database is empty or the room has no events in the database
                if (roomList.Count() == 0)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                    response.Content = new StringContent("No calendar entries found that room!", Encoding.UTF8);
                    return response;
                }
            }
            // Return a response indicating that the user entered an invalid room
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Invalid room or semester.");
                return response;
            }

            // Create a new list to hold the events for that semester and that room
            List<CalendarEventModel> rangeList = new List<CalendarEventModel>();
            string semester = Semester.ToLower();

            // Fall Semester
            if (semester == "fall")
            {
                DateTime startDay = new DateTime(DateTime.Now.Year, 8, 1);
                DateTime endDay = new DateTime(DateTime.Now.Year, 12, 31);

                // find events between august and decemeber
                foreach (CalendarEventModel ce in roomList)
                {
                    if (ce.StartTime.Date > startDay.Date && ce.StartTime.Date < endDay.Date)
                    {
                        rangeList.Add(ce);
                    }
                }
            }

            // Spring Semester
            else if (semester == "spring")
            {
                DateTime startDay = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime endDay = new DateTime(DateTime.Now.Year, 5, 31);

                // find events between january and may
                foreach (CalendarEventModel ce in roomList)
                {
                    if (ce.StartTime.Date > startDay.Date && ce.StartTime.Date < endDay.Date)
                    {
                        rangeList.Add(ce);
                    }
                }
            }

            // Summer Semester
            else if (semester == "summer")
            {
                DateTime startDay = new DateTime(DateTime.Now.Year, 5, 1);
                DateTime endDay = new DateTime(DateTime.Now.Year, 8, 31);

                // find events between may and august
                foreach (CalendarEventModel ce in roomList)
                {
                    if (ce.StartTime.Date > startDay.Date && ce.StartTime.Date < endDay.Date)
                    {
                        rangeList.Add(ce);
                    }
                }
            }

            // Invalid Semester
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Not a recognized semester.");
                return response;
            }

            // Check to see if there are any events for the given room and semester
            if (rangeList.Count() != 0)
            {
                // Set response to the list serialized into JSON
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(rangeList),
                    Encoding.UTF8, "application/json");
            }
            // Otherwise the database is empty or the room has no events in the database
            else
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("No calendar entries found for that room in that semester!", Encoding.UTF8);
                return response;
            }

            return response;

        }

        // test method to populate the database
        /*[HttpPost]
             public void InsertCalendarEvent(string eventTitle )
             {
                 CalendarEvent calendarEvent = new CalendarEvent();
                 calendarEvent.Title = eventTitle;
                 calendarEvent.StartTime = DateTime.Now;
                 calendarEvent.EndTime = DateTime.Now;
                 //calendarEvent.EventID = calID;

                 Random r = new Random();
                 int c = r.Next(1, 10);

                 if (c > 5)
                 {
                     calendarEvent.EventOwner = "Karlsson";
                 }
                 else
                 {
                     calendarEvent.EventOwner = "McGough";
                 }

                 calendarEventRepository.InsertCalendarEvent(calendarEvent);
                 calendarEventRepository.Save();
             } */

        /// <summary>
        /// Deletes all calendar events in the database
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage DeleteAllEvents()
        {
            HttpResponseMessage response;
            try
            {
                calendarEventRepository.DeleteAllCalendarEvents();
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("Opertation succeeded!");
            }
            catch
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Opertation failed");
            }

            // Returns whether or not the delete was successful
            return response;
        }
    }
}
