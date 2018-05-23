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
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response;

            int id = 0;
            var calendarEventList = new List<CalendarEventModel>
            {
                new CalendarEventModel { EventID = id++, Title = "Data Structures", StartTime = new DateTime(2017, 1, 23, 9, 00, 00), EndTime = new DateTime(2017, 1, 23, 10, 00, 00), EventOwner = "Hinker", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Data Structures", StartTime = new DateTime(2017, 1, 25, 9, 00, 00), EndTime = new DateTime(2017, 1, 25, 10, 00, 00), EventOwner = "Hinker", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Data Structures", StartTime = new DateTime(2017, 1, 26, 9, 00, 00), EndTime = new DateTime(2017, 1, 26, 10, 00, 00), EventOwner = "Hinker", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Data Structures", StartTime = new DateTime(2017, 1, 27, 9, 00, 00), EndTime = new DateTime(2017, 1, 27, 10, 00, 00), EventOwner = "Hinker", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Computer Science I", StartTime = new DateTime(2017, 1, 23, 12, 00, 00), EndTime = new DateTime(2017, 1, 23, 13, 00, 00), EventOwner = "McGough", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Computer Science I", StartTime = new DateTime(2017, 1, 25, 12, 00, 00), EndTime = new DateTime(2017, 1, 25, 13, 00, 00), EventOwner = "McGough", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Computer Science I", StartTime = new DateTime(2017, 1, 27, 12, 00, 00), EndTime = new DateTime(2017, 1, 27, 13, 00, 00), EventOwner = "McGough", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Analysis of Algorithms", StartTime = new DateTime(2017, 1, 23, 15, 00, 00), EndTime = new DateTime(2017, 1, 23, 16, 00, 00), EventOwner = "Lisa", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Analysis of Algorithms", StartTime = new DateTime(2017, 1, 25, 15, 00, 00), EndTime = new DateTime(2017, 1, 25, 16, 00, 00), EventOwner = "Lisa", Description = "Class" },
                new CalendarEventModel { EventID = id++, Title = "Analysis of Algorithms", StartTime = new DateTime(2017, 1, 27, 15, 00, 00), EndTime = new DateTime(2017, 1, 27, 16, 00, 00), EventOwner = "Lisa", Description = "Class" }
            };

            // return the calendar event entries with a status code of OK
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(calendarEventList),
                Encoding.UTF8, "application/json");
            return response;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "Values controller is up and running. You entered: " + id.ToString();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
