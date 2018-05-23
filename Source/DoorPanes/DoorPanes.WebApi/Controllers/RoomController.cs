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
    [Authorize]
    public class RoomController : ApiController
    {
        // instance of room repository
        private IRoomRepository roomRepository;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public RoomController()
        {
            this.roomRepository = new RoomRepository(new ApplicationDbContext());
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public RoomController(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        /// <summary>
        /// Endpoint to return all rooms in room table.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetAllRooms()
        {
            // get a list of calendar events
            var rooms = roomRepository.GetAllRooms().ToList();

            HttpResponseMessage response;

            // return no content status if no rooms are found
            if (rooms.Count == 0)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("No calendar entries found!", Encoding.UTF8);
                return response;
            }

            var sortedRooms = rooms.OrderBy(r => r.RoomNumber);

            // return the room entries with a status code of OK
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(sortedRooms),
                Encoding.UTF8, "application/json");
            return response;
        }
    }
}
