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
    public class FacultyController : ApiController
    {
        // private instance of faculty repository
        private IFacultyRepository facultyRepository;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FacultyController()
        {
            this.facultyRepository = new FacultyRepository(new ApplicationDbContext());
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="facultyRepository"></param>
        public FacultyController(IFacultyRepository facultyRepository)
        {
            this.facultyRepository = facultyRepository;
        }

        /// <summary>
        /// Returns a full list of all faculty members.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetFacultyMembers()
        {
            // get a list of faculty members
            var facultyMembers = facultyRepository.GetAllFacultyMembers().ToList();

            HttpResponseMessage response;

            // return no conent if no faculty members are found
            if (facultyMembers.Count == 0)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                response.Content = new StringContent("No faculty members found!", Encoding.UTF8);
                return response;
            }

            var sortedMembers = facultyMembers.OrderBy(f => f.FullName);

            // return the faculty member list with a status code of OK
            response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(sortedMembers),
                Encoding.UTF8, "application/json");
            return response;
        }
    }
}
