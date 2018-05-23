using DoorPanes.Services.DAL;
using DoorPanes.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoorPanes.WebApp.Controllers
{
    public class DashboardController : Controller
    {
        // private instance of cal event repo
        private ICalendarEventRepository calendarEventRepository;
        private IRoomRepository roomRepository;
        private string CurrentRoom
        {
            get
            {
                return (string)Session["CurrentRoom"] ?? null;
            }
            set
            {
                Session["CurrentRoom"] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DashboardController()
        {
            // initialize new repository instances
            calendarEventRepository = new CalendarEventRepository(new ApplicationDbContext());
            roomRepository = new RoomRepository(new ApplicationDbContext());            
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="calendarEventRepository">A Calendar Event Repository Instance</param>
        /// <param name="roomRepository">A room repository instance</param>
        public DashboardController(ICalendarEventRepository calendarEventRepository, IRoomRepository roomRepository)
        {
            // set repository instances
            this.calendarEventRepository = calendarEventRepository;
            this.roomRepository = roomRepository;
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            // only show dashboard view if user is logged in
            if (User.Identity.IsAuthenticated)
            {
                // set the current room for the user
                if (CurrentRoom == null)
                {
                    // return the room assinged to the owner by default
                    CurrentRoom = roomRepository.GetAllRooms()
                        .Where(r => r.RoomOwner == User.Identity.Name)
                        .Select(e => e.RoomNumber)
                        .ToList()
                        .FirstOrDefault();
                }

                // set the current room for the view
                ViewBag.CurrentRoom = CurrentRoom;

                return View();
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Returns a list of JSON full calendar events.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEvents(string Owner, string Room)
        {
            if (string.IsNullOrEmpty(Owner))
                return Json("Invalid user!", JsonRequestBehavior.AllowGet);

            // check for the right permissions to load events
            List<CalendarEventModel> calEvents = new List<CalendarEventModel>();
            if (User.IsInRole("Staff"))
            {
                calEvents = calendarEventRepository.GetCalendarEvents()
                    .Where(e => e.Room.RoomNumber == Room).ToList();
            }
            else if (User.IsInRole("Faculty"))
            {
                calEvents = calendarEventRepository.GetCalendarEvents()
                    .Where(e => e.EventOwner == Owner || e.Room.RoomNumber == Room).ToList();
            }
            else if (User.IsInRole("Student"))
            {
                calEvents = calendarEventRepository.GetCalendarEvents()
                    .Where(e => e.Room.RoomNumber == Room).ToList();
            }

            // create full calendar event list
            var fullCalEvents = new List<FullCalendarModel>();
            foreach (var calEvent in calEvents)
                fullCalEvents.Add(new FullCalendarModel(calEvent));

            // return the full calendar event list as serialized JSON
            return Json(fullCalEvents, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// POST endpoint for inserting calendar events created on calendar framework.
        /// </summary>
        [HttpPost]
        public ActionResult InsertCalendarEvent(int id, string title, string start, string end, string room, string owner, string color)
        {
            try
            {
                // build calendar model
                var calEvent = new CalendarEventModel(new FullCalendarModel(id, title, start, end, room, owner, color));

                // insert or update event in the dictionary
                if (!calendarEventRepository.Find(calEvent.EventID))
                    calendarEventRepository.InsertCalendarEvent(calEvent);
                else
                    calendarEventRepository.UpdateCalendarEvent(calEvent);
                calendarEventRepository.Save();
                return Json(new { success = true, responseText = "Saved event" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Insert failed. Reason:\n" + ex.Message);
                return Json(new { success = false, responseText = "Failed to save event" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Returns the next primary key id for calendar models.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNextId()
        {
            int nextId = 0;
            var eventList = calendarEventRepository.GetCalendarEvents();
            if (eventList.Count() > 0)
                nextId = eventList.Max(p => p.EventID) + 1;
            return Json(Convert.ToInt32(nextId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Endpoint to delete calendar events.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCalendarEvent(long id)
        {
            // delete the calendar event given id
            calendarEventRepository.DeleteCalendarEvent(id);
            calendarEventRepository.Save();
            return Json(new { success = true, responseText = "Deleted event" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Method to return the current user who is logged in.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCurrentUser()
        {
            // return the username of the current user
            return Json(User.Identity.Name, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the currently selected room number.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCurrentRoom()
        {
            return Json(CurrentRoom, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sets the current room session variable.
        /// </summary>
        /// <param name="roomNumber"></param>
        /// <returns></returns>
        public JsonResult RefreshForCurrentRoom(string roomNumber)
        {
            // set current room
            CurrentRoom = roomNumber;
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}